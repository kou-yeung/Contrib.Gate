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


    GetUser(context, (user) => {
        switch (s.command) {
            case "addcoin": addcoin(user); break;
        }
    }, false);

}
