// プロローグを完了する
function FinishPrologue(params, context, done) {

    GetUser(context, (user) => {
        new Entities.Player(user).bucket.refresh(player => {
            player.userCreateStep = UserCreateStep.Created;
            player.bucket.save(player => {
                // 返信
                let r = new FinishPrologueReceive();
                r.step = player.userCreateStep;
                done(r.Pack());
            });
        });
    });
}
