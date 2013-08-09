stache
======

stupid simple unopinonated templating.

place a .stache-src directory with all files using the stache engine inside.

stache templates are files that contain ._.

stache will build into the parent directory

for example:

<!-- language-all: lang-html -->
index.html

    <!DOCTYPE html>
    {{stuff}}
    </html>

  
stuff._.html

    <head>
    {{more-stuff}}
    </head>
    <body></body>
  
more-stuff._.html

    <script>alert('hello world');</script>

