// プロローグを完了する
function FinishPrologue(params, context, done) {
    // 受信データをパースする
    let s = FinishPrologueSend.Parse(params);

    // 選択できるペット一覧
    let pets = ["Familiar_001_001", "Familiar_001_016", "Familiar_001_017"];
    let id = Entities.Identify.Parse(pets[0]);

    GetUser(context, (user) => {

        // 使い魔追加
        new Entities.Pet(user).create(pet => {
            let item = pet.gen(id);
            pet.bucket.save(() => {
                // ユニットにセットする
                new Entities.Units(user).refresh(units => {
                    units.push(0, item.uniqid);
                    units.bucket.save(() => {
                        new Entities.Player(user).refresh(player => {
                            player.userCreateStep = UserCreateStep.Created;
                            player.bucket.save(player => {
                                // 返信
                                let r = new FinishPrologueReceive();
                                r.step = player.userCreateStep;
                                done(r.Pack());
                            });
                        });
                    });
                });
            });
        });
    });
}
