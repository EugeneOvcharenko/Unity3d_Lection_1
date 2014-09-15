Shader "Unlit/Transparent Color" {
	Properties {
		_MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
		_Color ("Color (RGBA)", color ) = ( 1, 0, 0, 1 )
	}

	SubShader {
		Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
		LOD 100
		
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha 

		Pass {
			Lighting Off
			SetTexture [_MainTex]
			{
				// присваиваем текущий цвет константе
				constantColor [_Color]
				// перемножаем значение цвета текстуры с константой
				combine texture * constant
			}
		}
	}
}