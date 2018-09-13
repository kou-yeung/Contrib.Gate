//====================
// ProtocolGen から自動生成されます。直接編集しないでください
//====================

class VendingSend {
    identify: number;

	// params -> VendingSend
	static Parse(params : any): VendingSend {
		return JSON.parse(params.data[0]) as VendingSend;
	}
}

class VendingReceive {
    identify: number;
    added: number;
    current: number;
    coin: number;
	
	// VendingReceive -> string
	Pack(): string {
		return JSON.stringify(this);
    }
}

/*
// 以下プロトコルを実装する
// MEMO : 直接にここに実装してしまうと、自動生成時に上書きされてしまいますのでご注意を!!!
function Vending(params, context, done) {

    // 受信データをパースする
    let s = VendingSend.Parse(params);

    GetUser(context, (user) => {
		// 返信
	    let r = new VendingReceive();

		// TODO : 返信パラメータを設定する

        done(r.Pack());
    });
}
*/