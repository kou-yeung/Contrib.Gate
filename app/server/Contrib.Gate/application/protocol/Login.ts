// Loginを受信する
function Login(params, context, done) {
    // 受信データをパースする
    let s = LoginSend.Parse(params);

    // context -> user
    GetUser(context, user => {
        new Entities.Player(user).bucket.refresh(player => {
            // 返信
            let r = new LoginReceive();
            r.step = player.userCreateStep;
            r.timestamp = Util.Time.ServerTime.current;

            done(r.Pack());
        });
    });
}