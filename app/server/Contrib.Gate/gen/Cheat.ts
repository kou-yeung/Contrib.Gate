//====================
// ProtocolGen から自動生成されます。直接編集しないでください
//====================

class CheatSend {
    command: string;
    param: string[];

	// params -> CheatSend
	static Parse(params : any): CheatSend {
		return JSON.parse(params.data[0]) as CheatSend;
	}
}

class CheatReceive {
    userState: UserState;
	
	// CheatReceive -> string
	Pack(): string {
		return JSON.stringify(this);
    }
}

/*
// 以下プロトコルを実装する
// MEMO : 直接にここに実装してしまうと、自動生成時に上書きされてしまいますのでご注意を!!!
function Cheat(params, context, done) {

    // 受信データをパースする
    let s = CheatSend.Parse(params);

    GetUser(context, (user) => {
		// 返信
	    let r = new CheatReceive();

		// TODO : 返信パラメータを設定する

        done(r.Pack());
    });
}
*/