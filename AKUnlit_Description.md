# AKUnlit 項目説明
## Rendering Mode
* Opaque - 完全に不透過なオブジェクトの描画用。
* Cutout - 透過・不透過かが明確なオブジェクトの描画用。切り抜き表現向きです。
* Transparent - 透過オブジェクトの描画用。反射などの映り込みは透過しません。
## Main Map
* Main -オブジェクト表面の見た目の設定です。
    * Albedo - オブジェクトの基本的な見た目。テクスチャや色で指定します。
    * Sacle Offset - テクスチャの繰り返し表示などの設定を行います。Albedo テクスチャと Emission テクスチャに反映されます。
## Options
* Rendering Options - 描画全般に関するオプションの設定です。
    * Culling Mask - オブジェクトの表面や裏面の表示設定でsj。
        * OFF - 表面裏面ともに描画します。
        * Front - 裏面のみ描画します。
        * Back - 表面のみ描画します。
    * Render Queue - 描画時の処理順を設定します。