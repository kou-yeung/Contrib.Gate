// 孵化
function Hatch(params, context, done) {

    // 受信データをパースする
    let s = HatchSend.Parse(params);

    //s.uniqid;
    GetUser(context, (user) => {

        new Entities.Egg(user, s.uniqid).refresh(egg => {
            //====================
            // MEMO : 将来は孵化完了したかどうかチェックを追加します。今はそのままPET にする
            //====================

            let guid = GUID.Gen();
            new Entities.Pet(user, guid).refresh(pet => {

                let item = new PetItem();
                item.id = egg.result.idWithType;
                item.uniqid = guid;
                item.createTime = Util.Time.ServerTime.current;
                item.param = [0, 0, 0, 0, 0, 0, 0, 0, 0, 0]; // param x 10

                pet.uniqid = guid;
                pet.item = item;

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
}
