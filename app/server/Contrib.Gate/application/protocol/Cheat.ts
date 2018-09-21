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
                egg.uniqid = guid;
                egg.result = id;
                egg.race = familiar.race;
                egg.rarity = familiar.rarity;
                egg.createTime = Util.Time.ServerTime.current;
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
        let lv = parseInt(s.param[1]);
        if (id.Type != IDType.Familiar) {
            done(ApiError.Create(ErrorCode.Common, "id.Type != IDType.Familiar").Pack());
            return;
        }
        lv = Math.max(lv, 1);

        // TODO : 未対応
    }

    GetUser(context, (user) => {
        switch (s.command) {
            case "addcoin": addcoin(user); break;
            case "addegg": addegg(user); break;
            case "addfamiliar": addfamiliar(user); break;
        }
    }, false);

}
