// パワーアップ
function Powerup(params, context, done) {

    // 受信データをパースする
    let s = PowerupSend.Parse(params);
    let powerupCount = 0;
    s.param.forEach(num => powerupCount += num);

    var itemId = new Entities.Identify(2001, IDType.Material);

    GetUser(context, user => {
        // インベントリ一覧取得
        new Entities.Inventory(user).refresh(inventory => {

            // 所持アイテム数をチェックする
            if (powerupCount > inventory.num(itemId)) {
                done(ApiError.Create(ErrorCode.Common, "強化の魂が足りません").Pack());
                return;
            }

            // 強化対象ペットを取得する
            new Entities.Pet(user, s.uniqid).refresh(pet => {

                // 返信データ
                let r = new PowerupReceive();
                r.items = [];
                r.pet = pet.item;

                // 一時対応:古いデータにセットされてなかったため
                if (r.pet.powerupCount == undefined || r.pet.powerupCount == null) r.pet.powerupCount = 0;
                let canPowerupCount = (r.pet.level * 4) - r.pet.powerupCount;

                // 回数チェック
                if (canPowerupCount < powerupCount) {
                    done(ApiError.Create(ErrorCode.Common, "パワーアップできる回数が超えてました").Pack());
                    return;
                }

                // 回数更新
                r.pet.powerupCount += powerupCount;
                // パラメータ更新
                for (var i = 0; i < s.param.length; i++) {
                    r.pet.param[i] += s.param[i];
                }
                // アイテムを使用する
                let remain = inventory.add(itemId, -powerupCount);

                let item = new InventoryItem();
                item.identify = itemId.idWithType;
                item.num = remain;
                r.items.push(item);
                // 更新
                pet.item = r.pet;

                inventory.bucket.save(() => {
                    pet.bucket.save(() => {
                        done(r.Pack());
                    });
                });
            });
        });
    });
}


