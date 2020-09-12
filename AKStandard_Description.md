# AKStandard 項目説明
## Rendering Mode
* Opaque - 完全に不透過なオブジェクトの描画用。
* Cutout - 透過・不透過かが明確なオブジェクトの描画用。切り抜き表現向きです。
* Fade - 透過オブジェクトの描画用。反射などの映り込みも透過します。
* Transparent - 透過オブジェクトの描画用。反射などの映り込みは透過しません。
## Main Map
* Main -オブジェクト表面の見た目の設定です。
    * Albedo - オブジェクトの基本的な見た目。テクスチャや色で指定します。
    * Normal Map - オブジェクトの表面の光の当たり方を、テクスチャを元に擬似的に表現します。スライダーで強度を調整できます。
    * Hight Map - オブジェクトの表面の凸凹による遮蔽を、テクスチャを元に擬似的に表現します。スライダーで強度を調整できます。
    * Occlusion - オブジェクト表面の光による影の付き方を、テクスチャを元に表現します。スライダーで強度を調整できます。
* Emission - オブジェクトの表面の発光の設定です。
    * Color - 発光の色を、テクスチャや色で指定します。カラーパレットの Intensity から輝度を調整できます。
    * Global Illumination - 発光が周囲へ与える影響を設定します。設定はライトベイクでのみ効果があります。
        * Realtime - 発光はリアルタイム周辺光源の計算に使用されます。Static 設定がされていないオブジェクトを照らせるようになります。
        * Baked - 発光はライトベイクの際に周囲の Static オブジェクトへ焼き込まれます。それ以外のオブジェクトは、LightProbe の設定によってのみ照らされます。
* Reflection - オブジェクト表面の反射・映り込みの設定です。
    * Metallic - オブジェクト表面がどれくらい金属的な反射をするか設定します。テクスチャとスライダーで強度を設定できます。
    * Smoothness - オブジェクト表面がどれだけ滑らかかを設定します。テクスチャとスライダーで強度を設定できます。次の Source も合わせて参照してください。
    * Source - Smoothness の元となるテクスチャの参照元を設定します。
        * Albedo Alpha - Albedo テクスチャの Alpha 値を使用します。
        * Metallic Alpha - Metallic テクスチャの Alpha 値を使用します。
        * Roughness Map - Smoothness に指定したテクスチャを使用します。
* Sacle Offset - テクスチャの繰り返し表示などの設定を行います。Albedo テクスチャと Emission テクスチャに反映されます。
## Secondary Map
* Detail - オブジェクトの見た目の設定の二つ目です。主にキャラクターの皮膚のディテールのの表現などに使用します。
    * Detail Mask - ディテールを表示するエリアをマスクテクスチャを使用して制御します。
    * Detail Albedo - ディテールを表示するテクスチャです。
    * Detail Normal - ディテール用のノーマルマップです。強度はスライダーではなく数値で指定します。
    * Tilling Offset - ディテールの繰り返し表示などの設定を行います。
    * UV Set - ディテールの表示箇所を決定する UV マップを指定します。
## Options
* Forward Rendering Options - オブジェクトの表面描画のオプションの設定です。
    * Specular Highlights - 鏡面反射のハイライトを使用するかどうか。
    * Reflection - 反射の映り込みを使用するかどうか。
* Rendering Options - 描画全般に関するオプションの設定です。
    * Culling Mask - オブジェクトの表面や裏面の表示設定でsj。
        * OFF - 表面裏面ともに描画します。
        * Front - 裏面のみ描画します。
        * Back - 表面のみ描画します。
    * Render Queue - 描画時の処理順を設定します。
* Advanced Options
    * Enable GPU Instancing - 同一メッシュをまとめて描画する機能です。[詳細](https://docs.unity3d.com/ja/2018.4/Manual/GPUInstancing.html)
    * Doubel Sided Global Illumination - 環境光源を計算する際に、オブジェクトの両面を使用します。