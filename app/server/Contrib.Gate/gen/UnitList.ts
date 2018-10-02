//====================
// ProtocolGen から自動生成されます。直接編集しないでください
//====================

class UnitListSend {


	// params -> UnitListSend
	static Parse(params : any): UnitListSend {
		return JSON.parse(params.data[0]) as UnitListSend;
	}
}

class UnitListReceive {
    items: UnitItem[]; // Unit情報一覧
	
	// UnitListReceive -> string
	Pack(): string {
		return JSON.stringify(this);
    }
}

/*
// 以下プロトコルを実装する
// MEMO : 直接にここに実装してしまうと、自動生成時に上書きされてしまいますのでご注意を!!!
function UnitList(params, context, done) {

    // 受信データをパースする
    let s = UnitListSend.Parse(params);

    GetUser(context, (user) => {
		// 返信
	    let r = new UnitListReceive();

		// TODO : 返信パラメータを設定する

        done(r.Pack());
    });
}
*/