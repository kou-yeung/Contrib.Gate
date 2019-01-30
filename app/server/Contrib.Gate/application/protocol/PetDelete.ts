// ペット削除

function PetDelete(params, context, done) {

    // 受信データをパースする
    let s = PetDeleteSend.Parse(params);

    let admin = GetAdmin(context);

    GetUser(context, (user) => {
        new Entities.Pet(user, s.uniqid).refresh(pet => {

            let item = pet.item;

            new Entities.Familiar(admin).refresh([new Entities.Identify(item.id)], familiar => {
                if (!pet.valid) {
                    done(ApiError.Create(ErrorCode.Common, "不明なID").Pack());
                    return;
                }
                new Entities.Units(user).refresh(units => {
                    // 使用中
                    if (units.exists(s.uniqid)) {
                        done(ApiError.Create(ErrorCode.Common, "ユニットに配置しています").Pack());
                        return;
                    }
                    new Entities.Inventory(user).refresh(inventory => {

                        // 返信
                        let r = new PetDeleteReceive();
                        r.items = [];

                        // 強化の魂を追加
                        {
                            var itemId = new Entities.Identify(2001, IDType.Material);  // 強化の魂
                            var data = new InventoryItem();
                            data.identify = itemId.idWithType;
                            data.num = inventory.add(itemId, familiar.rarity);
                            r.items.push(data);
                        }

                        // 所持スキルを戻し
                        if (item.skill != 0)
                        {
                            var itemId = new Entities.Identify(item.skill);
                            var data = new InventoryItem();
                            data.identify = itemId.idWithType;
                            data.num = inventory.add(itemId, 1);
                            r.items.push(data);
                        }

                        // ペット削除
                        pet.bucket.first.delete(() => {
                            inventory.bucket.save(() => {
                                done(r.Pack());
                            });
                        });
                    });
                });
            });
        })
    });
}
