// ゲーム内のユーザデータを生成する
function CreateUser(params, context, done) {
    // 受信データをパースする
    let s = CreateUserSend.Parse(params);

    GetUser(context, user => {
        new Entities.Player(user).refresh(player => {

            user.update({}, {
                success: function () {
                    player.UserName = s.name;
                    player.userCreateStep = UserCreateStep.Prologue;

                    player.bucket.save(player => {
                        // 返信
                        let r = new CreateUserReceive();
                        r.step = player.userCreateStep;
                        done(r.Pack());
                    });
                },
                failure: function () {
                }
            },{ "displayName": s.name });
        });
    });
}