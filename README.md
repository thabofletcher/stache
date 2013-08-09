stache
======

stupid simple unopinonated templating.

place a .stache-src directory with all files using the stache engine inside.
stache templates are files that contain ._.

stache will build into the parent directory

for example:

index.html
  <pre>
  <!DOCTYPE html>
  {{stuff}}
  </pre>
  
stuff._.html
  <pre>
  <head>
  {{more-stuff}}
  </head>
  <body></body>
  </pre>
  
more-stuff._.html
  <pre>
  <script>alert('hello world');</script>
  </pre>
