//====================
// ProtocolGen から自動生成されます。直接編集しないでください
//====================

class FinishPrologueSend {


	// params -> FinishPrologueSend
	static Parse(params : any): FinishPrologueSend {
		return JSON.parse(params.data[0]) as FinishPrologueSend;
	}
}

class FinishPrologueReceive {
    step: UserCreateStep;
	
	// FinishPrologueReceive -> string
	Pack(): string {
		return JSON.stringify(this);
    }
}

/*
// 以下プロトコルを実装する
// MEMO : 直接にここに実装してしまうと、自動生成時に上書きされてしまいますのでご注意を!!!
function FinishPrologue(params, context, done) {

    // 受信データをパースする
    let s = FinishPrologueSend.Parse(params);

    GetUser(context, (user) => {
		// 返信
	    let r = new FinishPrologueReceive();

		// TODO : 返信パラメータを設定する

        done(r.Pack());
    });
}
*/