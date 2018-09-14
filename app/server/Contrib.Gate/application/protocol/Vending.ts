function Vending(params, context, done) {

    // 受信データをパースする
    let s = VendingSend.Parse(params);

    if (new Entities.Identify(s.identify).Type != IDType.Vending) {
        // 無効な自販機ID
        let r = new ApiError(ErrorCode.VendingInvalid);
        done(r.Pack());
    }

    let admin = GetAdmin(context);
    new Entities.Vending(admin, new Entities.Identify(s.identify)).refresh(vending =>
    {
        GetUser(context, (user) => {

            // TODO : vending.level で自販機レベルチェック
            new Entities.Player(user).bucket.refresh(player =>
            {
                if (player.coin < vending.price) {
                    // コインが足りない
                    let r = new ApiError(ErrorCode.CoinLack);
                    done(r.Pack());
                } else {
                    player.coin -= vending.price;
                    player.bucket.save(() => {
                        // 正常処理
                        new Entities.Inventory(user).refresh(inventory => {
                            // 返信
                            let r = new VendingReceive();
                            r.identify = vending.target.idWithType;
                            r.added = vending.num;
                            r.current = inventory.add(vending.target, vending.num);
                            r.coin = player.coin;
                            inventory.bucket.save(() => done(r.Pack()));
                        });
                    });
                }
            });
        });
    });
}
