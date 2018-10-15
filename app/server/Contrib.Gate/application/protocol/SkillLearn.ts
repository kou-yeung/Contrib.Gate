// スキル取得
function SkillLearn(params, context, done) {

    // 受信データをパースする
    let s = SkillLearnSend.Parse(params);
    let id = new Entities.Identify(s.skill);
    // IDの種類が正しいか？
    if (id.Type != IDType.Skill) {
        done(ApiError.Create(ErrorCode.Common, "スキルIDではありません").Pack());
        return;
    }
    GetUser(context, (user) => {
        new Entities.Inventory(user).refresh(inventory => {
            var remain = inventory.add(id, -1);
            // 消費アイテムがない
            if (remain < 0) {
                done(ApiError.Create(ErrorCode.Common, "指定スキルが持っていない").Pack());
                return;
            }

            new Entities.Pet(user, s.uniqid).refresh(pet => {
                if (!pet.valid) {
                    done(ApiError.Create(ErrorCode.Common, "指定ペットが持っていない").Pack());
                    return;
                }

                // 返信
                let r = new SkillLearnReceive();

                // 更新後のアイテム数
                r.item = new InventoryItem();
                r.item.identify = s.skill;
                r.item.num = remain;

                // ペット
                r.pet = pet.item;
                r.pet.skill = s.skill;
                pet.item = r.pet;

                // 保存
                inventory.bucket.save(() => {
                    pet.bucket.save(() => {
                        done(r.Pack());
                    });
                });
            });
        });
    });
}
