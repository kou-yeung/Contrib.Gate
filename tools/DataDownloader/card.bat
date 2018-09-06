SET sheetid=1d7jfkxK57fePSuMaU6DbdoAheERUcKR4VYJvlncgvKQ
SET apikey=AIzaSyChKdUxx0lPQsb_LF08bJ0XqVlvS4b5aKw
SET sheetname=card
SET dataStart=3

CALL DataDownloader.exe sheetid=%sheetid% apikey=%apikey% sheetname=%sheetname% flagLine=1 dataStart=%dataStart% output="./card_sv.csv"
CALL DataDownloader.exe sheetid=%sheetid% apikey=%apikey% sheetname=%sheetname% flagLine=2 dataStart=%dataStart% output="./card_cl.csv"

pause