function Inventory(params, context, done) {

    // 受信データをパースする
    let s = InventorySend.Parse(params);

    GetUser(context, (user) => {
        new Entities.Inventory(user).refresh(inventory => {
            // 返信
            let r = new InventoryReceive();
            r.items = [];

            let obj = inventory.bucket.first;
            let keys = obj.allkey;

            for (var i = 0; i < keys.length; i++) {
                let num: number = obj.get(keys[i]);
                if (num <= 0) continue;
                let item = new InventoryItem();
                item.identify = Entities.Identify.Parse(keys[i]).idWithType;
                item.num = num;
                r.items.push(item);
            }
            done(r.Pack());
        });
    }, false);
}
