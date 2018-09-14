//====================
// ProtocolGen から自動生成されます。直接編集しないでください
//====================

class LoginSend {


	// params -> LoginSend
	static Parse(params : any): LoginSend {
		return JSON.parse(params.data[0]) as LoginSend;
	}
}

class LoginReceive {
    timestamp: number; // // サーバ時間
    step: UserCreateStep;
    iv: number[];
    key: number[];
	
	// LoginReceive -> string
	Pack(): string {
		return JSON.stringify(this);
    }
}

/*
// 以下プロトコルを実装する
// MEMO : 直接にここに実装してしまうと、自動生成時に上書きされてしまいますのでご注意を!!!
function Login(params, context, done) {

    // 受信データをパースする
    let s = LoginSend.Parse(params);

    GetUser(context, (user) => {
		// 返信
	    let r = new LoginReceive();

		// TODO : 返信パラメータを設定する

        done(r.Pack());
    });
}
*/