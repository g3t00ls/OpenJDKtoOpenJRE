# OpenJDKtoOpenJRE
All rights reserved
(still need to chose a license/write one)

Converts and compresses(zip) OpenJDK to OpenJRE for smaller distribution.

Therefore it scans all OpenJDK modules, filters them, and than packs them into OpenJRE using jlink from OpenJDK.

Requirements:
```sh
.Net 2.0
```

Size:
```sh
~ 14 kb
```


Help:
```sh
/help
```

Convert:
```sh
/convert "<OpenJDKdir>" "<OpenJREdir_output>"
```
No " \ " @ end of dir!
Will create subfolder:
```sh
"jre-*VersionNumber*" @ "*OpenJREdir_output*"!
```

Usage from cmd:
```sh
OpenJDKtoOpenJRE.exe /convert "<OpenJDKdir>" "<OpenJREdir_output>"
```
