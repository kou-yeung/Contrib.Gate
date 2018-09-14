SET sheetid=1d7jfkxK57fePSuMaU6DbdoAheERUcKR4VYJvlncgvKQ
SET apikey=AIzaSyChKdUxx0lPQsb_LF08bJ0XqVlvS4b5aKw
SET sheetname=string_table
SET dataStart=3
SET crypt_iv=Q7V10/1qKgkV61CGk25PKA==
SET crypt_key=FYxcgJgSVVrO5hMGd+zehpM8FI/y8e0+eAJ+KQKsmZg=

CALL DataDownloader.exe sheetid=%sheetid% apikey=%apikey% sheetname=%sheetname% flagLine=2 dataStart=%dataStart% output="./../../app/client/Contrib.Gate/Assets/Resources/Entities/"
