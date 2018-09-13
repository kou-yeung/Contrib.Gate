

// 参考 : https://docs.kii.com/ja/guides/serverextension/writing_servercode/executing-user/
// context -> user
function GetUser(context, done: (user: KiiUser) => void, auth: boolean = false): void{

    if (auth) {
        KiiUser.authenticateWithToken(context.getAccessToken(), {
            success: function (theAuthedUser) {
                done(theAuthedUser);
            },
            failure: function (theUser, error) {
            }
        });
    } else {
        done(GetAdmin(context).userWithID(context.userID));
    }
}
// context -> 管理者取得
function GetAdmin(context): KiiAppAdminContext {
    return context.getAppAdminContext() as KiiAppAdminContext;   // AdminContext 取得
}


