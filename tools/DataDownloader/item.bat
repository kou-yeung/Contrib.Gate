SET sheetid=1d7jfkxK57fePSuMaU6DbdoAheERUcKR4VYJvlncgvKQ
SET apikey=AIzaSyChKdUxx0lPQsb_LF08bJ0XqVlvS4b5aKw
SET sheetname=item
SET dataStart=4
SET crypt_iv=Q7V10/1qKgkV61CGk25PKA==
SET crypt_key=FYxcgJgSVVrO5hMGd+zehpM8FI/y8e0+eAJ+KQKsmZg=

CALL DataDownloader.exe sheetid=%sheetid% apikey=%apikey% sheetname=%sheetname% flagLine=2 dataStart=%dataStart% output="./../../app/client/Contrib.Gate/Assets/Resources/Entities/" crypt_iv=%crypt_iv% crypt_key=%crypt_key%
CALL DataDownloader.exe sheetid=%sheetid% apikey=%apikey% sheetname=%sheetname% flagLine=1 dataStart=%dataStart% output="./../../resources/sv/Entities/"
