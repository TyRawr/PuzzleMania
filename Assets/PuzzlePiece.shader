/*
	In order for this shader to work you'll have to set each pieces uv in code
	to the correct piece of the full image. Something that looks like this:

	for (int i = 0; i < rows; i++)
	{
		for (int j = 0; j < cols; j++)
		{
			Mesh mesh = piece.GetComponent<MeshFilter>().mesh;

			Vector2[] uvs = mesh.uv;
			uvs[0] = new Vector2((j - 1) * uvWidth, (i - 1) * uvHeight);
			uvs[3] = new Vector2((j - 1) * uvWidth, (i + 2) * uvHeight);
			uvs[1] = new Vector2((j + 2) * uvWidth, (i + 2) * uvHeight);
			uvs[2] = new Vector2((j + 2) * uvWidth, (i - 1) * uvHeight);
			mesh.uv = uvs;
		}
	}
*/

Shader "Custom/Puzzle Piece" {
	Properties{
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_Mask("_Mask", 2D) = "white" {}
		_Mask2("_Mask2", 2D) = "white" {}
		_Mask3("_Mask3", 2D) = "white" {}
		_Mask4("_Mask4", 2D) = "white" {}
	}

	SubShader{
		Tags{ "RenderType" = "Transparent" }
		LOD 200

		CGPROGRAM
#pragma surface surf Lambert alpha
#pragma target 3.0
		sampler2D _MainTex;
		sampler2D _Mask;
		sampler2D _Mask2;
		sampler2D _Mask3;
		sampler2D _Mask4;

		struct Input {
			// UVs that give each piece it's part of the full image.
			float2 uv_MainTex;

			// UVs that allow each mask to ignore the previous UVs and retain their full image.
			float2 uv2_Mask;
			float2 uv2_Mask2;
			float2 uv2_Mask3;
		};

		fixed4 _Color;
		void surf(Input IN, inout SurfaceOutput o) {
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;

			// My system works with vertical masks only.
			// If the art team wants a new connector then instead of creating 
			// four .pngs they would only need to create one.
			float2 uv_TOP = IN.uv2_Mask;
			// Rotates the mask to its proper orientation.
			float2 uv_Right = IN.uv2_Mask2;
			float2 uv_Bot = IN.uv2_Mask;
			float2 uv_Left = IN.uv2_Mask;


			


			// Puts them all together.
			o.Alpha = c.a * tex2D(_Mask, uv_TOP).r  * tex2D(_Mask2, uv_Right).r  * tex2D(_Mask3, uv_Bot ) * tex2D(_Mask4 ,uv_Left );
		}

		ENDCG
	}

	FallBack "Diffuse"
}