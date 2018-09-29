// 孵化予約

function HatchReserve(params, context, done) {

    // 受信データをパースする
    let s = HatchReserveSend.Parse(params);

    GetUser(context, (user) => {
        // タマゴデータを取得する
        new Entities.Egg(user, s.uniqid).refresh(egg => {

            if (!egg.valid) {
                // 指定されたIDは存在しません
                done(ApiError.Create(ErrorCode.Common, "指定されたIDは存在しません").Pack());
                return;
            }

            new Entities.Hatch(user).refresh(hatchs => {
                if (hatchs.bucket.results.length >= Const.MaxHatch) {
                    // 同時に孵化できるのは3つまでです
                    done(ApiError.Create(ErrorCode.HatchMax).Pack());
                    return;
                }

                new Entities.Hatch(user, s.uniqid).refresh(hatch => {
                    // すでに孵化中？
                    if (hatch.bucket.first.has("item")) {
                        done(ApiError.Create(ErrorCode.Common, "このIDはすでに孵化中です").Pack());
                        return;
                    }

                    let item = new HatchItem();
                    item.startTime = Util.Time.ServerTime.current;
                    item.timeRequired = egg.item.rarity * (30 * 60); // レアリティ 1 x 30分
                    item.uniqid = s.uniqid;

                    hatch.uniqid = s.uniqid;
                    hatch.item = item;

                    hatch.bucket.save(() => {
                        // 返信
                        let r = new HatchReserveReceive();
                        r.item = hatch.item;
                        done(r.Pack());
                    });
                });
            });
        });
    });
}
