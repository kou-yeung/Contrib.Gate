function Vending(params, context, done) {

    // 受信データをパースする
    let s = VendingSend.Parse(params);

    let admin = GetAdmin(context);
    new Entities.Vending(admin, new Entities.Identify(s.identify)).refresh(vending =>
    {
        GetUser(context, (user) => {
            new Entities.Inventory(user).refresh(inventory =>
            {
                // 返信
                let r = new VendingReceive();
                r.identify = vending.target.idWithType;
                r.added = vending.num;
                r.current = inventory.add(vending.target, vending.num);
                inventory.bucket.save(() => done(r.Pack()));
            });
        });
    });
}
