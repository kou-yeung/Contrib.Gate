// サーバ時間の取得

namespace Util.Time {
    export class ServerTime {
        // サーバー時間を取得する
        // MEMO : JavaScript では ミリ秒まで取得されるため、最後の3桁は削除しています!!
        static get current(): number {
            var str = Date.now().toString().slice(0, -3);
            return parseInt(str);
            //Number.isSafeInteger(parseInt(ThisInt))
        }
    }
}
