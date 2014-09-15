Shader "Unlit/Transparent Vertex Color" {
	Properties {
		_MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
	}

	SubShader {
		Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
		LOD 100
		
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha 

		Pass
		{
			ColorMaterial AmbientAndDiffuse
	            
			Lighting Off
			
			SetTexture [_MainTex]
			{
				// складываем значение primary цвета, т.е. цвета вершины
				// и значения полученные из текстуры
				combine primary * texture
			}
		}
	}
}