import os
import site

pipeline = os.path.abspath(os.path.dirname(__file__))
sitedir = os.path.join(pipeline, 'site-packages')

site.addsitedir(pipeline)
site.addsitedir(sitedir)
