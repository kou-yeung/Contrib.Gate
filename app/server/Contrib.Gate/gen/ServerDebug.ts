//====================
// ProtocolGen から自動生成されます。直接編集しないでください
//====================

class ServerDebugSend {
    command: string;

	// params -> ServerDebugSend
	static Parse(params : any): ServerDebugSend {
		return JSON.parse(params.data[0]) as ServerDebugSend;
	}
}

class ServerDebugReceive {
    param: string;
    context: string;
    message: string;
	
	// ServerDebugReceive -> string
	Pack(): string {
		return JSON.stringify(this);
    }
}

/*
// 以下プロトコルを実装する
// MEMO : 直接にここに実装してしまうと、自動生成時に上書きされてしまいますのでご注意を!!!
function ServerDebug(params, context, done) {

    // 受信データをパースする
    let s = ServerDebugSend.Parse(params);

    GetUser(context, (user) => {
		// 返信
	    let r = new ServerDebugReceive();

		// TODO : 返信パラメータを設定する

        done(r.Pack());
    });
}
*/