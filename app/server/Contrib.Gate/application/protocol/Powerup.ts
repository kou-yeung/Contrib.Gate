// パワーアップ
function Powerup(params, context, done) {

    // 受信データをパースする
    let s = PowerupSend.Parse(params);

    // ID一覧作成
    let ids: Entities.Identify[] = [];
    s.items.forEach(item => ids.push(new Entities.Identify(item.identify)));

    let admin = GetAdmin(context);
    // アイテム情報取得
    new Entities.Item(admin).refresh(ids, items => {

        // 餌の増加回数を計算する
        let powerupCount = 0;
        s.items.forEach(item => {
            powerupCount += items.effects[item.identify].powerup * item.num;
        });

        GetUser(context, (user) => {
            new Entities.Pet(user, s.uniqid).refresh(pet => {

                // 返信
                let r = new PowerupReceive();
                r.items = [];

                r.pet = pet.item;
                if (r.pet.powerupCount == undefined || r.pet.powerupCount == null) r.pet.powerupCount = 0;  // 一時対応:古いデータにセットされてなかったため
                let canPowerupCount = r.pet.level - r.pet.powerupCount;
                // 回数チェック
                if (canPowerupCount < powerupCount) {
                    done(ApiError.Create(ErrorCode.Common, "パワーアップできる回数が超えてました").Pack());
                    return;
                }
                new Entities.Inventory(user).refresh(inventory => {
                    for (var i = 0; i < ids.length; i++) {
                        let remain = inventory.add(ids[i], -s.items[i].num);
                        if (remain < 0) {
                            done(ApiError.Create(ErrorCode.Common, "消費アイテムが足りません").Pack());
                            return;
                        }
                        // 指定IDのアイテム情報を取得する
                        let item = items.find(s.items[i].identify);
                        // 餌の回数を加算する : アイテム毎の回数 * アイテム数
                        r.pet.powerupCount = item.powerup * s.items[i].num;
                        // パラメータ増加
                        item.param.forEach((param, i) => {
                            r.pet.param[param] += item.value[i] * s.items[i].num;
                        });

                        s.items[i].num = remain;    // 使いまわす!!
                        r.items.push(s.items[i]);   // 返信データに追加
                    }
                    pet.item = r.pet;   // 更新
                    inventory.bucket.save(() => {
                        pet.bucket.save(() => {
                            done(r.Pack());
                        });
                    });
                });
            });
        });
    });
}


