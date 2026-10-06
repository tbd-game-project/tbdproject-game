# 画面周辺エフェクト

画面の周辺に模様付きのエフェクトを表示する機能。
現在の見た目は霧状で、表示条件やスキルとの連携は、利用側で実装する。

## 使用するファイル

- `Shaders/ScreenEdgeFog.shader`：画面端の範囲・模様・色を計算して描画する。
- `Materials/ScreenEdgeFogMaterial.mat`：上記Shaderを使用するMaterial。
- `Scripts/ScreenEdgeEffectController.cs`：Inspectorと公開関数から表示・見た目を操作する。

## 導入手順

1. 使用するURP Renderer Dataに「Full Screen Pass Renderer Feature」を追加する。
2. Feature内のNameを `ScreenEdgeFog` に設定する。
3. Injection Pointを `After Rendering Post Processing` に設定する。
4. Requirementsを `None`、Fetch Color Bufferをオン、Bind Depth-Stencilをオフにする。
5. Pass Materialに `ScreenEdgeFogMaterial` を設定する。Pass Indexが表示される場合は `0` にする。
6. 対象Cameraが、このRendererを使用するように設定する。
7. Sceneに空のGameObjectを作り、`ScreenEdgeEffectController` を追加する。
8. ControllerのRenderer Dataに、手順1のRenderer Dataを設定する。
9. ControllerのFeature Nameを `ScreenEdgeFog` に設定する。

新しいRenderer Dataを使用する場合は、使用中のURP AssetのRenderer Listへの登録が必要。
共有RendererやURP Assetを変更するときは、担当者と調整。

## Inspectorの設定

| 項目 | 用途 |
|---|---|
| Renderer Data | 操作するFeatureが登録されているRenderer |
| Feature Name | 操作するFull Screen PassのName |
| Effect Enabled | 表示・非表示の切り替え |
| Effect Color | 色。Alphaを下げると薄くなる |
| Intensity | 濃さ。0〜1 |
| Edge Width | 画面端からの広がり。0〜0.5 |
| Softness | 境界の柔らかさ |
| Noise Scale | 模様の細かさ。大きいほど細かくなる |
| Noise Strength | 模様による濃淡。0で均一になる |

Controllerが動作している間は、Controllerの設定がMaterialへ反映される。
見た目の調整はController側で行う。

## 他のスクリプトからの操作

操作元のスクリプトで、Controllerへの参照を持たせる。

```csharp
[SerializeField]
private ScreenEdgeEffectController screenEdgeEffect;
```

Inspectorで、この欄に対象のControllerを設定する。

```csharp
screenEdgeEffect.Show();              // 表示
screenEdgeEffect.Hide();              // 非表示
screenEdgeEffect.SetIntensity(0.6f);  // 濃さ
screenEdgeEffect.SetColor(Color.blue); // 色
screenEdgeEffect.SetEdgeWidth(0.2f);  // 広がり
```

公開関数は、再生中に有効なControllerへ呼び出すこと。
スキルの開始・終了など、呼び出すタイミングは利用側で管理する。

## 動作と注意点

- 実行中はMaterialを複製し、元のMaterialへの変更を避ける。
- 非表示時は、対象のFull Screen Passを停止する。
- Controllerを無効にすると、開始前のMaterialとFeatureの有効状態へ戻る。
- 同じFeatureを操作するControllerは1つにすること。
- 同じRendererを使う複数のCameraに影響する。Cameraごとに独立して操作する機能は含みまない。
- このエフェクトは画面への重ね合わせ。立体的な霧や背景のぼかしではない。

## 確認済みの内容

- 個人用Sceneで画面周辺に表示できること。
- 再生中にControllerのInspectorから表示と見た目を変更できること。
- 別のスクリプトからHideとShowを呼び、非表示・再表示できること。

スキルとの連携、複数Cameraでの利用、統合先Sceneでの動作は未確認。