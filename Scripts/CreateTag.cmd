
rem//  $LastChangedDate: 2016-01-29 17:03:12 +0100 (pt., 29 sty 2016) $
rem//  $Rev: 11855 $
rem//  $LastChangedBy: mpostol $
rem//  $URL: http://svn.server300161.nazwa.pl/cas/VS/trunk/PR44-SharePoint/Scripts/CreateTag.cmd $
rem//  $Id: CreateTag.cmd 11855 2016-01-29 16:03:12Z mpostol $


set branchtype=tags
set TagPath=svn://svnserver.hq.cas.com.pl/VS/%branchtype%/CAS.SharePoint.rel_2_61_7
set trunkPath=svn://svnserver.hq.cas.com.pl/VS/trunk

svn mkdir %TagPath%  -m "created new %TagPath% (in %branchtype% folder)"

svn copy %trunkPath%/ImageLibrary %TagPath%/ImageLibrary -m "created copy in %TagPath% of the project ImageLibrary"
svn copy %trunkPath%/PR39-CommonResources %TagPath%/PR39-CommonResources -m "created copy in %TagPath% of the project PR39-CommonResources"
svn copy %trunkPath%/PR44-SharePoint %TagPath%/PR44-SharePoint -m "created copy in %TagPath% of the project PR44-SharePoint"

pause ....

