using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using CAS.SharePoint.Setup.Controls;
using CAS.SharePoint.Setup.Package;
using CAS.SharePoint.Setup.SharePoint;
using CAS.SharePoint.Tools.Packaging;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;

namespace CAS.SharePoint.Setup.StateMachine
{
  internal static class Commands
  {
    internal static void DoValidation(Package.InstallationStateData installationStateData, BackgroundWorker _wrkr, DoWorkEventArgs e)
    {
      e.Result = true;
      try
      {
        if (_wrkr.CancellationPending)
        {
          e.Cancel = true;
          e.Result = false;
          return;
        }
        FarmHelpers.GetFarm();
        string _msg = string.Empty;
        if (FarmHelpers.Farm != null)
        {
          _msg = String.Format(Properties.Resources.FarmGotAccess, FarmHelpers.Farm.Name, FarmHelpers.Farm.DisplayName, FarmHelpers.Farm.Status);
          _wrkr.ReportProgress(1, _msg);
        }
        else
          throw new ApplicationException(Properties.Resources.GettingAccess2LocalFarm);
        Uri _url = installationStateData.WebApplicationUri;
        FarmHelpers.GetWebApplication(_url);
        if (FarmHelpers.WebApplication != null)
        {
          _msg = String.Format(Properties.Resources.ApplicationFound, _url, FarmHelpers.WebApplication.Name, FarmHelpers.WebApplication.DisplayName);
          _wrkr.ReportProgress(1, _msg);
        }
        else
          throw new ApplicationException(String.Format(Properties.Resources.GettingAccess2ApplicationFailed, _url));
        string _urlSite = installationStateData.SiteCollectionURL;
        bool _spsiteExist = FarmHelpers.WebApplication.Sites.Names.Contains(_urlSite);
        if (_wrkr.CancellationPending)
        {
          e.Cancel = true;
          e.Result = false;
          return;
        }
        if (installationStateData.SiteCollectionCreated)
        {
          if (_spsiteExist)
            _wrkr.ReportProgress(1, Properties.Resources.SiteExistAndReuse);
          else
          {
            installationStateData.SiteCollectionCreated = false;
            UriBuilder _spuri = new UriBuilder(_url) { Path = _urlSite };
            string _siteNotExist = String.Format("Cannot get access to the installed site collection at URL = {0}", _spuri);
            throw new ApplicationException(_siteNotExist);
          }
        }
        else
          if (_spsiteExist)
          {
            //SiteCollectionHelper.SiteCollection = FarmHelpers.WebApplication.Sites[m_ApplicationState.SiteCollectionURL];
            string _ms = String.Format(Properties.Resources.SiteCollectionExist, _urlSite);
            SiteCollectionHelper.DeleteIfExist =
              MessageBox.Show(_ms, Properties.Resources.SiteCreation, MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes;
            if (SiteCollectionHelper.DeleteIfExist)
              _wrkr.ReportProgress(1, Properties.Resources.SiteExistAndDelete);
            else
              _wrkr.ReportProgress(1, Properties.Resources.SiteExistAndReuse);
          }
        _wrkr.ReportProgress(1, Properties.Resources.ValidationProcessSuccessfullyFinished);
      }
      catch (Exception ex)
      {
        string _msg = String.Format(Properties.Resources.ValidationProcessFailed, ex.Message);
        _wrkr.ReportProgress(1, _msg);
        e.Result = false;
      }
    }
    internal static void DoInstallation(Package.InstallationStateData installationStateData, BackgroundWorker _wrkr, DoWorkEventArgs e, FileInfo packageContentLocation)
    {
      SPSecurity.RunWithElevatedPrivileges(delegate()
        {
          if (_wrkr.CancellationPending)
          {
            e.Cancel = true;
            e.Result = false;
            return;
          }
          try
          {
            string _format = "Trying to create or reuse the site collection at {0}, configured site template {1}, LCID {2}.";
            _wrkr.ReportProgress(1, String.Format(_format, installationStateData.SiteCollectionURL, installationStateData.SiteTemplate, installationStateData.LCID));
            using (SiteCollectionHelper m_SiteCollectionHelper = SiteCollectionHelper.GetSPSite(
              FarmHelpers.WebApplication,
              installationStateData.SiteCollectionURL,
              installationStateData.Title,
              installationStateData.Description,
              installationStateData.LCID,
              null,
              installationStateData.OwnerLogin,
              installationStateData.OwnerName,
              installationStateData.OwnerEmail))
            {
              installationStateData.SiteCollectionCreated = true;
              _wrkr.ReportProgress(1, "Site collection is created");
              if (m_SiteCollectionHelper.NewTemplateRequired)
              {
                _wrkr.ReportProgress(1, "Applying new template - select required from dialog.");
                s_EndOFUIAction.Reset();
                SPWebTemplateCollection _cllctn = m_SiteCollectionHelper.GetWebTemplates(installationStateData.LCID);
                _wrkr.ReportProgress(1, new SPWebTeplateEventArgs(_cllctn, GetTemplate, 1, "Getting Site template"));  //GetTemplate( FarmHelpers.WebApplication.Sites[ _urlSite ], installationStateData );
                s_EndOFUIAction.WaitOne();
                _wrkr.ReportProgress(1, String.Format("Applying new template {0}", WebTemplateDialog.SPWebTemplateToString(installationStateData.SPWebTemplate)));
                m_SiteCollectionHelper.ApplayTemplate(installationStateData.SPWebTemplate);
              }
              _format = "The site template is configured to Name: {0}/ Id: {1}.";
              _wrkr.ReportProgress(1, String.Format(_format, m_SiteCollectionHelper.SiteTemplate, m_SiteCollectionHelper.SiteTemplateID));
              using (PackageContent _PackageContent = new PackageContent(packageContentLocation))
              {
                foreach (Solution _sltn in installationStateData.SolutionsToInstall.Values)
                {
                  FileInfo _fi = _sltn.SolutionFileInfo(_PackageContent.ContentLocation);
                  _wrkr.ReportProgress(1, String.Format("Deploying solution: {0}", _fi.Name));
                  switch (_sltn.FeatureDefinitionScope)
                  {
                    case FeatureDefinitionScope.Farm:
                      TimeSpan _timeout = new TimeSpan(0, 0, Properties.Settings.Default.SolutionDeploymentTimeOut);
                      string _waitingForCompletion = String.Format("Waiting for completion .... It could take up to {0} s. ", _timeout);
                      _wrkr.ReportProgress(1, _waitingForCompletion);
                      SPSolution _sol = null;
                      if (_sltn.Global)
                        _sol = FarmHelpers.DeploySolution(_fi, _timeout);
                      else
                        _sol = FarmHelpers.DeploySolution(_fi, FarmHelpers.WebApplication, _timeout);
                      _sltn.SolutionGuid = _sol.Id;
                      _wrkr.ReportProgress(1, String.Format("Solution deployed Name={0}, Deployed={1}, DeploymentState={2}, DisplayName={3} Status={4}", _sol.Name, _sol.Deployed, _sol.DeploymentState, _sol.DisplayName, _sol.Status));
                      break;
                    case FeatureDefinitionScope.Site:
                      SPUserSolution _solution = null;
                      _solution = m_SiteCollectionHelper.DeploySolution(_fi);
                      _sltn.SolutionGuid = _solution.SolutionId;
                      _wrkr.ReportProgress(1, String.Format("Solution deployed: {0}", _solution.Name));
                      break;
                    case FeatureDefinitionScope.None:
                    default:
                      throw new ApplicationException("Wrong FeatureDefinitionScope in the configuration file");
                  }
                  _sltn.Deployed = true;
                  foreach (Feature _fix in _sltn.Fetures)
                  {
                    bool _repeat;
                    do
                    {
                      _repeat = false;
                      try
                      {
                        if (!_fix.AutoActivate)
                        {
                          _wrkr.ReportProgress(1, String.Format("Skipping activation of the feature: {0} at: {1} because tha activation is set false", _fix.FetureGuid, m_SiteCollectionHelper.SiteCollection.Url));
                          break;
                        }
                        _wrkr.ReportProgress(1, String.Format("Activating Feature: {0} at: {1}", _fix.FetureGuid, m_SiteCollectionHelper.SiteCollection.Url));
                        SPFeature _ffeature = m_SiteCollectionHelper.ActivateFeature(_fix.FetureGuid, _sltn.SPFeatureDefinitionScope);
                        _wrkr.ReportProgress(1, String.Format("Feature activated : {0}", _ffeature.Definition.DisplayName));
                        _fix.DisplayName = _ffeature.Definition.DisplayName;
                        _fix.Version = _ffeature.Version.ToString();
                        _fix.SPScope = _ffeature.Definition.Scope;
                      }
                      catch (Exception ex)
                      {
                        string _msg = String.Format(Properties.Resources.FeatureActivationFailureMBox, ex.Message);
                        Tracing.TraceEvent.TraceError(516, "SetUpData.Install", _msg);
                        switch (MessageBox.Show(_msg, "Install ActivateFeature", MessageBoxButton.YesNoCancel, MessageBoxImage.Error, MessageBoxResult.No))
                        {
                          case MessageBoxResult.Cancel:
                            throw ex;
                          case MessageBoxResult.Yes:
                            _repeat = true;
                            break;
                          default:
                            break;
                        }
                      }
                    } while (_repeat);
                  }//foreach (Feature _fix in _sltn.Fetures)
                  _sltn.Activated = true;
                }
              } //foreach (Solution _sltn 
              //TODO installationStateData.Save();
              _wrkr.ReportProgress(1, "Product installation successfully completed");
            }
          }
          catch (Exception ex)
          {
            //TODO
            //try
            //{
            //  installationStateData.Save();
            //}
            //catch ( Exception _SaveEx )
            //{
            //  InstallationListBox.AddMessage( _SaveEx.Message );
            //}
            string _msg = String.Format(Properties.Resources.LastOperationFailedWithError, ex.Message);
            _wrkr.ReportProgress(1, _msg);
            throw ex;
          }
        });
    }
    private static System.Threading.ManualResetEvent s_EndOFUIAction = new ManualResetEvent(true);
    private static void GetTemplate()
    {
      s_EndOFUIAction.Set();
    }
  }
}
