function Cheat(params, context, done) {

    // 受信データをパースする
    let s = CheatSend.Parse(params);

    let admin = GetAdmin(context);
    // TODO : ユーザID がチートコマンドの実行可能かどうかチェックする


    // コイン増加
    function addcoin(user: KiiUser) {
        new Entities.Player(user).refresh(player => {
            player.coin += parseInt(s.param[0]);
            player.bucket.save(() => {
                var r = new CheatReceive();
                r.userState = [player.userState];
                done(r.Pack());
            });
        });
    }

    // タマゴ追加
    function addegg(user: KiiUser) {
        let id = Entities.Identify.Parse(s.param[0]);
        let judgment = false;
        if (s.param.length >= 2) {
            s.param[1].toLowerCase() == "true";
        }

        // ID が違う
        if (id.Type != IDType.Familiar) {
            done(ApiError.Create(ErrorCode.Common, "id.Type != IDType.Familiar").Pack())
            return;
        }
        new Entities.Familiar(admin).refresh([id], familiar => {
            let guid = GUID.Gen();  // 新たなGUIDを生成する
            new Entities.Egg(user, guid).refresh(egg => {
                let item = new EggItem();
                if (judgment) {
                    item.race = familiar.race;
                    item.rarity = familiar.rarity;
                }
                item.createTime = Util.Time.ServerTime.current;
                item.uniqid = guid;
                item.judgment = judgment;

                egg.uniqid = guid;
                egg.result = id;
                egg.item = item;
                egg.bucket.save(() => {
                    var r = new CheatReceive();
                    r.egg = [item];
                    done(r.Pack());
                });
            });
        });
    }

    // 使い魔追加
    function addfamiliar(user: KiiUser) {
        let id = Entities.Identify.Parse(s.param[0]);
        let lv = parseInt(s.param[1].length <= 0 ? "1" : s.param[1]);
        if (id.Type != IDType.Familiar) {
            done(ApiError.Create(ErrorCode.Common, "id.Type != IDType.Familiar").Pack());
            return;
        }

        lv = Math.max(lv, 1);
        new Entities.Level(admin).refresh(level => {
            new Entities.Pet(user).create(pet => {
                let item = pet.gen(id, lv, level.exp(lv));
                pet.bucket.save(() => {
                    var r = new CheatReceive();
                    r.pet = [item];
                    done(r.Pack());
                });
            });
        });
    }


    // アイテム追加
    function additem(user: KiiUser) {
        let id = Entities.Identify.Parse(s.param[0]);
        let num = parseInt(s.param[1].length <= 0 ? "1" : s.param[1]);

        switch (id.Type) {
            case IDType.Material:
            case IDType.Item:
            case IDType.Skill:
                break;
            default:
                done(ApiError.Create(ErrorCode.Common, "追加できないIDTypeです").Pack());
                return;
        }

        new Entities.Inventory(user).refresh(inventory => {
            var remain = inventory.add(id, num);

            inventory.bucket.save(() => {
                let item = new InventoryItem();
                item.identify = id.idWithType;
                item.num = remain;
                var r = new CheatReceive();
                r.items = [item];
                done(r.Pack());
            });
        });
    }
    GetUser(context, (user) => {
        switch (s.command) {
            case "addcoin": addcoin(user); break;
            case "addegg": addegg(user); break;
            case "addfamiliar": addfamiliar(user); break;
            case "additem": additem(user); break;
        }
    }, false);

}
