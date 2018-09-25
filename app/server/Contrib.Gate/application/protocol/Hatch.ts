// 孵化
function Hatch(params, context, done) {

    // 受信データをパースする
    let s = HatchSend.Parse(params);

    //s.uniqid;
    GetUser(context, (user) => {

        new Entities.Hatch(user, s.uniqid).refresh(hatch => {

            // データの取得ができなかった
            if (!hatch.valid) {
                done(ApiError.Create(ErrorCode.Common, "無効な孵化ID").Pack());
                return;
            }
            // 時間がまだ足りない
            let remain = (hatch.item.startTime + hatch.item.timeRequired) - Util.Time.ServerTime.current;
            if (remain > 0) {
                done(ApiError.Create(ErrorCode.Common, "まだ時間が足りない").Pack());
                return;
            }

            new Entities.Egg(user, s.uniqid).refresh(egg => {

                let guid = GUID.Gen();
                new Entities.Pet(user, guid).refresh(pet => {

                    let item = new PetItem();
                    item.id = egg.result.idWithType;
                    item.uniqid = guid;
                    item.createTime = Util.Time.ServerTime.current;
                    item.param = [];
                    for (var i = 0; i < Param.Count; i++) item.param.push(0);

                    pet.uniqid = guid;
                    pet.item = item;

                    // 孵化情報削除
                    hatch.bucket.first.delete(() => {
                        // タマゴ削除
                        egg.bucket.first.delete(() => {
                            // ペット保存
                            pet.bucket.save(() => {
                                // 返信
                                let r = new HatchReceive();
                                r.item = item;
                                r.deleteEgg = egg.item;
                                done(r.Pack());
                            });
                        });
                    });
                });
            });
        });
    });
}
