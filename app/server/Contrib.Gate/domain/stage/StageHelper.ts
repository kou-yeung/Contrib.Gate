class StageHelper {

    // ダンジョンID から生成時のシード値を取得する
    static StageSeed(dungeonId: Entities.Identify): number {
        var time = new Date();   // 現在の時刻を取得する
        // 2時間間隔固定する
        time.setHours(Math.floor(time.getHours() / 2) * 2); // 時間は最後の偶数にする
        time.setMinutes(0); // 分は 0 
        time.setSeconds(0); // 秒は 0 
        time.setMilliseconds(0); // ミリ秒は 0 
        // 桁を反転する
        let str = time.getTime().toString();
        let base = parseInt(str.split("").reverse().join(""));
        return base + dungeonId.Id;
    }

    // ステージの消失時間を取得
    static StageLossTime(): number {
        var time = new Date();   // 現在の時刻を取得する
        // 2時間間隔固定する
        time.setHours(Math.floor(time.getHours() / 2) * 2); // 時間は最後の偶数にする
        time.setMinutes(0); // 分は 0 
        time.setSeconds(0); // 秒は 0 
        time.setMilliseconds(0); // ミリ秒は 0 

        // 固定された時間の2時間先
        time.setHours(time.getHours() + 2);

        var str = time.getTime().toString().slice(0, -3);
        return parseInt(str);
    }
}