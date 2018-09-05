

// 参考 : https://docs.kii.com/ja/guides/serverextension/writing_servercode/executing-user/
// context -> user
function GetUser(context, done: (user: KiiUser) => void): void{
    KiiUser.authenticateWithToken(context.getAccessToken(), {
        success: function (theAuthedUser) {
            done(theAuthedUser);
        },
        failure: function (theUser, error) {
        }
    });
}
// context -> 管理者取得
function GetAdmin(context, done: (admin: KiiAppAdminContext) => void): void {
    done(context.getAppAdminContext() as KiiAppAdminContext);   // AdminContext 取得
}


