function Cheat(params, context, done) {

    // 受信データをパースする
    let s = CheatSend.Parse(params);

    let admin = GetAdmin(context);
    // TODO : ユーザID がチートコマンドの実行可能かどうかチェックする


    // コイン増加
    function addcoin(user: KiiUser) {
        new Entities.Player(user).bucket.refresh(player => {
            player.coin += parseInt(s.param[0]);
            player.bucket.save(() => {
                var r = new CheatReceive();
                r.userState = player.userState;
                done(r.Pack());
            });
        });
    }

    // タマゴ追加
    function addegg(user: KiiUser) {
        let id = Entities.Identify.Parse(s.param[0]);
        // ID が違う
        if (id.Type != IDType.Familiar) {
            done(ApiError.Create(ErrorCode.Common, "id.Type != IDType.Familiar").Pack())
            return;
        }
        new Entities.Familiar(admin, id).refresh(familiar => {
            let guid = GUID.Gen();  // 新たなGUIDを生成する
            new Entities.Egg(user, guid).refresh(egg => {
                let item = new EggItem();
                item.race = familiar.race;
                item.rarity = familiar.rarity;
                item.createTime = Util.Time.ServerTime.current;
                item.uniqid = guid;

                egg.uniqid = guid;
                egg.item = item;
                egg.result = id;
                egg.bucket.save(() => {
                    var r = new CheatReceive();
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
            let guid = GUID.Gen();
            new Entities.Pet(user, guid).refresh(pet => {

                let item = new PetItem();
                item.id = id.idWithType;
                item.uniqid = guid;
                item.createTime = Util.Time.ServerTime.current;
                item.param = [level.exp(lv), 2, 0, 0, 0, 0, 0, 0, 0, 0]; // param x 10

                pet.uniqid = guid;
                pet.item = item;

                pet.bucket.save(() => {
                    var r = new CheatReceive();
                    done(r.Pack());
                });
            });
        });
    }

    GetUser(context, (user) => {
        switch (s.command) {
            case "addcoin": addcoin(user); break;
            case "addegg": addegg(user); break;
            case "addfamiliar": addfamiliar(user); break;
        }
    }, false);

}
