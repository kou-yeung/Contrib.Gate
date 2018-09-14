//====================
// ProtocolGen から自動生成されます。直接編集しないでください
//====================

class InventorySend {


	// params -> InventorySend
	static Parse(params : any): InventorySend {
		return JSON.parse(params.data[0]) as InventorySend;
	}
}

class InventoryReceive {
    items: InventoryItem[]; // 所持アイテム一覧
	
	// InventoryReceive -> string
	Pack(): string {
		return JSON.stringify(this);
    }
}

/*
// 以下プロトコルを実装する
// MEMO : 直接にここに実装してしまうと、自動生成時に上書きされてしまいますのでご注意を!!!
function Inventory(params, context, done) {

    // 受信データをパースする
    let s = InventorySend.Parse(params);

    GetUser(context, (user) => {
		// 返信
	    let r = new InventoryReceive();

		// TODO : 返信パラメータを設定する

        done(r.Pack());
    });
}
*/