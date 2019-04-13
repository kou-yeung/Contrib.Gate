// Loginを受信する
function Login(params, context, done) {
    // 受信データをパースする
    let s = LoginSend.Parse(params);

    let admin = GetAdmin(context);
    new Entities.Config(admin).refresh(config => {
        // context -> user
        GetUser(context, user => {
            new Entities.Player(user).refresh(player => {
                // 返信
                let r = new LoginReceive();
                r.appVersion = config.appVersion;
                r.userState = player.userState;
                r.timestamp = Util.Time.ServerTime.current;
                config.crypt = r.timestamp; // 暗号化タイミング設定
                r.iv = config.iv;
                r.key = config.key;
                done(r.Pack());
            });
        });
    });
}