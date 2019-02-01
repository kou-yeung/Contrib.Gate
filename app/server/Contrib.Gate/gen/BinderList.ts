//====================
// ProtocolGen から自動生成されます。直接編集しないでください
//====================

class BinderListSend {


	// params -> BinderListSend
	static Parse(params : any): BinderListSend {
		return JSON.parse(params.data[0]) as BinderListSend;
	}
}

class BinderListReceive {
    ids: string[]; // 取得済ペットのID一覧
	
	// BinderListReceive -> string
	Pack(): string {
		return JSON.stringify(this);
    }
}

/*
// 以下プロトコルを実装する
// MEMO : 直接にここに実装してしまうと、自動生成時に上書きされてしまいますのでご注意を!!!
function BinderList(params, context, done) {

    // 受信データをパースする
    let s = BinderListSend.Parse(params);

    GetUser(context, (user) => {
		// 返信
	    let r = new BinderListReceive();

		// TODO : 返信パラメータを設定する

        done(r.Pack());
    });
}
*/