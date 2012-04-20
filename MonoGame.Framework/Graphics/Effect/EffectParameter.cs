using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Microsoft.Xna.Framework.Graphics
{
    [DebuggerDisplay("{ParameterClass} {ParameterType} {Name} : {Semantic}")]
	public class EffectParameter
	{
        internal EffectParameter(   EffectParameterClass class_, 
                                    EffectParameterType type, 
                                    string name, 
                                    int rowCount, 
                                    int columnCount, 
                                    string semantic, 
                                    EffectAnnotationCollection annotations,
                                    EffectParameterCollection elements,
                                    EffectParameterCollection structMembers,
                                    object data )
		{
            ParameterClass = class_;
            ParameterType = type;

            Name = name;
            Semantic = semantic;
            Annotations = annotations;

#if WINRT

#else
        internal ActiveUniformType activeUniformType;

		internal DXEffectObject.D3DXPARAMETER_TYPE rawType;
		internal GLSLEffectObject.glslEffectParameterType rawGLSLType;

        public static EffectParameterType GetEffectParameterType(ActiveUniformType glType)
        {
            switch (glType)
            {
                case ActiveUniformType.Float:
                case ActiveUniformType.FloatVec2:
                case ActiveUniformType.FloatVec3:
                case ActiveUniformType.FloatVec4:
                case ActiveUniformType.FloatMat2:
                case ActiveUniformType.FloatMat3:
                case ActiveUniformType.FloatMat4:
                    return EffectParameterType.Single;
                case ActiveUniformType.Int:
                case ActiveUniformType.IntVec2:
                case ActiveUniformType.IntVec3:
                case ActiveUniformType.IntVec4:
                    return EffectParameterType.Int32;
                case ActiveUniformType.Bool:
                case ActiveUniformType.BoolVec2:
                case ActiveUniformType.BoolVec3:
                case ActiveUniformType.BoolVec4:
                    return EffectParameterType.Bool;
                case ActiveUniformType.Sampler2D:
                    return EffectParameterType.Texture2D;
                case ActiveUniformType.SamplerCube:
                    return EffectParameterType.TextureCube;
            }
            throw new NotSupportedException();
        }

        public static EffectParameterClass GetEffectParameterClass(ActiveUniformType glType)
        {
            switch (glType)
            {
                case ActiveUniformType.Float:
                case ActiveUniformType.Int:
                case ActiveUniformType.Bool:
                    return EffectParameterClass.Scalar;
                case ActiveUniformType.FloatVec2:
                case ActiveUniformType.FloatVec3:
                case ActiveUniformType.FloatVec4:
                case ActiveUniformType.IntVec2:
                case ActiveUniformType.IntVec3:
                case ActiveUniformType.IntVec4:
                case ActiveUniformType.BoolVec2:
                case ActiveUniformType.BoolVec3:
                case ActiveUniformType.BoolVec4:
                    return EffectParameterClass.Vector;
                case ActiveUniformType.FloatMat2:
                case ActiveUniformType.FloatMat3:
                case ActiveUniformType.FloatMat4:
                    return EffectParameterClass.Matrix;
                case ActiveUniformType.Sampler2D:
                case ActiveUniformType.SamplerCube:
                    return EffectParameterClass.Object;
            }
            throw new NotSupportedException();
        }


        internal EffectParameter(ActiveUniformType type, string uniformName)
        {
            this.activeUniformType = type;
            this.paramClass = GetEffectParameterClass(type);
            this.paramType = GetEffectParameterType(type);
            this.name = uniformName;

            switch (type)
            {
                case ActiveUniformType.Float:
                    this.data = default(Single); break;
                case ActiveUniformType.Int:
                    this.data = default(int); break;
                case ActiveUniformType.Bool:
                    this.data = default(bool); break;
                case ActiveUniformType.FloatVec2:
                    this.data = default(Vector2); break;
                case ActiveUniformType.FloatVec3:
                    this.data = default(Vector3); break;
                case ActiveUniformType.FloatVec4:
                    this.data = default(Vector4); break;
                case ActiveUniformType.IntVec2:
                    this.data = new int[2]; break;
                case ActiveUniformType.IntVec3:
                    this.data = new int[3]; break;
                case ActiveUniformType.IntVec4:
                    this.data = new int[4]; break;
                case ActiveUniformType.BoolVec2:
                    this.data = new bool[2]; break;
                case ActiveUniformType.BoolVec3:
                    this.data = new bool[3]; break;
                case ActiveUniformType.BoolVec4:
                    this.data = new bool[4]; break;
                case ActiveUniformType.FloatMat2:
                case ActiveUniformType.FloatMat3:
                    this.rowCount = 3;
                    this.colCount = 3;
                    this.data = default(Matrix);
                    break;
                case ActiveUniformType.FloatMat4:
                    this.rowCount = 4;
                    this.colCount = 4;
                    this.data = default(Matrix);
                    break;
                case ActiveUniformType.Sampler2D:
                case ActiveUniformType.SamplerCube:
                    this.data = null; break;
            }
        }

        internal EffectParameter( DXEffectObject.d3dx_parameter parameter )
		{
			switch (parameter.class_) {
			case DXEffectObject.D3DXPARAMETER_CLASS.SCALAR:
				paramClass = EffectParameterClass.Scalar;
				break;
			case DXEffectObject.D3DXPARAMETER_CLASS.VECTOR:
				paramClass = EffectParameterClass.Vector;
				break;
			case DXEffectObject.D3DXPARAMETER_CLASS.MATRIX_ROWS:
			case DXEffectObject.D3DXPARAMETER_CLASS.MATRIX_COLUMNS:
				paramClass = EffectParameterClass.Matrix;
				break;
			case DXEffectObject.D3DXPARAMETER_CLASS.OBJECT:
				paramClass = EffectParameterClass.Object;
				break;
			case DXEffectObject.D3DXPARAMETER_CLASS.STRUCT:
				paramClass = EffectParameterClass.Struct;
				break;
			default:
				throw new NotImplementedException();
			}
			
			switch (parameter.type) {
			case DXEffectObject.D3DXPARAMETER_TYPE.VOID:
				paramType = EffectParameterType.Void;
				break;
			case DXEffectObject.D3DXPARAMETER_TYPE.BOOL:
				paramType = EffectParameterType.Bool;
				break;
			case DXEffectObject.D3DXPARAMETER_TYPE.INT:
				paramType = EffectParameterType.Int32;
				break;
			case DXEffectObject.D3DXPARAMETER_TYPE.FLOAT:
				paramType = EffectParameterType.Single;
				break;
			case DXEffectObject.D3DXPARAMETER_TYPE.STRING:
				paramType = EffectParameterType.String;
				break;
			case DXEffectObject.D3DXPARAMETER_TYPE.TEXTURE:
				paramType = EffectParameterType.Texture;
				break;
			case DXEffectObject.D3DXPARAMETER_TYPE.TEXTURE1D:
				paramType = EffectParameterType.Texture1D;
				break;
			case DXEffectObject.D3DXPARAMETER_TYPE.TEXTURE2D:
				paramType = EffectParameterType.Texture2D;
				break;
			case DXEffectObject.D3DXPARAMETER_TYPE.TEXTURE3D:
				paramType = EffectParameterType.Texture3D;
				break;
			case DXEffectObject.D3DXPARAMETER_TYPE.TEXTURECUBE:
				paramType = EffectParameterType.TextureCube;
				break;
			default:
				//we need types not normally exposed in XNA for internal use
				rawParameter = true;
				break;
			}
			rawType = parameter.type;
			
			name = parameter.name;
			rowCount = (int)parameter.rows;
			colCount = (int)parameter.columns;
			semantic = parameter.semantic;
			
			annotations = new EffectAnnotationCollection();
			for (int i=0; i<parameter.annotation_count; i++) {
				EffectAnnotation annotation = new EffectAnnotation();
				annotations._annotations.Add (annotation);
			}
			
			elements = new EffectParameterCollection();
			structMembers = new EffectParameterCollection();
			if (parameter.element_count > 0) {
				for (int i=0; i<parameter.element_count; i++) {
					EffectParameter element = new EffectParameter(parameter.member_handles[i]);
					elements._parameters.Add (element);
				}
			} else if (parameter.member_count > 0) {
				for (int i=0; i<parameter.member_count; i++) {
					EffectParameter member = new EffectParameter(parameter.member_handles[i]);
					structMembers._parameters.Add (member);
				}
			} else {
				if (rawParameter) {
					data = parameter.data;
				} else {
					//interpret data
					switch (paramClass) {
					case EffectParameterClass.Scalar:
						switch (paramType) {
						case EffectParameterType.Bool:
							data = BitConverter.ToBoolean((byte[])parameter.data, 0);
							break;
						case EffectParameterType.Int32:
							data = BitConverter.ToInt32 ((byte[])parameter.data, 0);
							break;
						case EffectParameterType.Single:
							data = BitConverter.ToSingle((byte[])parameter.data, 0);
							break;
						case EffectParameterType.Void:
							data = null;
							break;
						default:
							break;
							//throw new NotSupportedException();
						}
						break;
					case EffectParameterClass.Vector:
					case EffectParameterClass.Matrix:
						switch (paramType) {
						case EffectParameterType.Single:
							float[] vals = new float[rowCount*colCount];
							//transpose maybe?
							for (int i=0; i<rowCount*colCount; i++) {
								vals[i] = BitConverter.ToSingle ((byte[])parameter.data, i*4);
							}
							data = vals;
							break;
						default:
							break;
						}
						break;
					default:
						//throw new NotSupportedException();
						break;
					}
				}
			}
			
		}

		internal EffectParameter( GLSLEffectObject.glslParameter parameter )
		{
			switch (parameter.class_) {
			case GLSLEffectObject.glslEffectParameterClass.Scalar:
				paramClass = EffectParameterClass.Scalar;
				break;
			case GLSLEffectObject.glslEffectParameterClass.Vector:
				paramClass = EffectParameterClass.Vector;
				break;
			case GLSLEffectObject.glslEffectParameterClass.MatrixRows:
			case GLSLEffectObject.glslEffectParameterClass.MatrixColumns:
				paramClass = EffectParameterClass.Matrix;
				break;
			case GLSLEffectObject.glslEffectParameterClass.Object:
				paramClass = EffectParameterClass.Object;
				break;
			case GLSLEffectObject.glslEffectParameterClass.Struct:
				paramClass = EffectParameterClass.Struct;
				break;
			default:
				throw new NotImplementedException();
			}

			switch (parameter.type) {
			case GLSLEffectObject.glslEffectParameterType.Void:
				paramType = EffectParameterType.Void;
				break;
			case GLSLEffectObject.glslEffectParameterType.Bool:
				paramType = EffectParameterType.Bool;
				break;
			case GLSLEffectObject.glslEffectParameterType.Int32:
				paramType = EffectParameterType.Int32;
				break;
			case GLSLEffectObject.glslEffectParameterType.Single:
				paramType = EffectParameterType.Single;
				break;
			case GLSLEffectObject.glslEffectParameterType.String:
				paramType = EffectParameterType.String;
				break;
			case GLSLEffectObject.glslEffectParameterType.Texture:
				paramType = EffectParameterType.Texture;
				break;
			case GLSLEffectObject.glslEffectParameterType.Texture1D:
				paramType = EffectParameterType.Texture1D;
				break;
			case GLSLEffectObject.glslEffectParameterType.Texture2D:
				paramType = EffectParameterType.Texture2D;
				break;
			case GLSLEffectObject.glslEffectParameterType.Texture3D:
				paramType = EffectParameterType.Texture3D;
				break;
			case GLSLEffectObject.glslEffectParameterType.TextureCube:
				paramType = EffectParameterType.TextureCube;
				break;
			case GLSLEffectObject.glslEffectParameterType.Sampler:
				paramType = EffectParameterType.Texture;
				break;
			case GLSLEffectObject.glslEffectParameterType.Sampler1D:
				paramType = EffectParameterType.Texture1D;
				break;
			case GLSLEffectObject.glslEffectParameterType.Sampler2D:
				paramType = EffectParameterType.Texture2D;
				break;
			case GLSLEffectObject.glslEffectParameterType.Sampler3D:
				paramType = EffectParameterType.Texture3D;
				break;
			case GLSLEffectObject.glslEffectParameterType.SamplerCube:
				paramType = EffectParameterType.TextureCube;
				break;
			default:
				//we need types not normally exposed in XNA for internal use
				rawParameter = true;
				break;
			}
			rawGLSLType = parameter.type;
			
			name = parameter.name;
			rowCount = (int)parameter.rows;
			colCount = (int)parameter.columns;
			semantic = parameter.semantic;
			
			annotations = new EffectAnnotationCollection();
			for (int i=0; i<parameter.annotation_count; i++) {
				EffectAnnotation annotation = new EffectAnnotation();
				annotation.Name = (string)parameter.annotation_handles[i].data;
				annotations._annotations.Add (annotation);
			}
			
			elements = new EffectParameterCollection();
			structMembers = new EffectParameterCollection();
			if (parameter.element_count > 0) {
				for (int i=0; i<parameter.element_count; i++) {
					EffectParameter element = new EffectParameter(parameter.member_handles[i]);
					elements._parameters.Add (element);
				}
			} else if (parameter.member_count > 0) {
				for (int i=0; i<parameter.member_count; i++) {
					EffectParameter member = new EffectParameter(parameter.member_handles[i]);
					structMembers._parameters.Add (member);
				}
			} else {
				if (rawParameter) {
					data = parameter.data;
				} else {
					//interpret data
					switch (paramClass) {
					case EffectParameterClass.Scalar:
						switch (paramType) {
						case EffectParameterType.Bool:
							data = BitConverter.ToBoolean((byte[])parameter.data, 0);
							break;
						case EffectParameterType.Int32:
							data = BitConverter.ToInt32 ((byte[])parameter.data, 0);
							break;
						case EffectParameterType.Single:
							data = BitConverter.ToSingle((byte[])parameter.data, 0);
							break;
						case EffectParameterType.Void:
							data = null;
							break;
						case EffectParameterType.Texture:
						case EffectParameterType.Texture1D:
						case EffectParameterType.Texture2D:
						case EffectParameterType.Texture3D:
						case EffectParameterType.TextureCube:
							data = null;
							break;

						default:
							break;
							//throw new NotSupportedException();
						}
						break;
					case EffectParameterClass.Vector:
					case EffectParameterClass.Matrix:
						switch (paramType) {
						case EffectParameterType.Single:
							float[] vals = new float[rowCount*colCount];
							//transpose maybe?
							for (int i=0; i<rowCount*colCount; i++) {
								vals[i] = BitConverter.ToSingle ((byte[])parameter.data, i*4);
							}
							data = vals;
							break;
						default:
							break;
						}
						break;
					default:
						throw new NotSupportedException();
						break;
					}
				}
			}
			
		}
#endif

        public int ColumnCount {
			get { return colCount; }
		}

            Data = data;
		}

        internal EffectParameter(EffectParameter cloneSource)
        {
            // Share all the immutable types.
            ParameterClass = cloneSource.ParameterClass;
            ParameterType = cloneSource.ParameterType;
            Name = cloneSource.Name;
            Semantic = cloneSource.Semantic;
            Annotations = cloneSource.Annotations;
            RowCount = cloneSource.RowCount;
            ColumnCount = cloneSource.ColumnCount;
            
            // Clone the mutable types.
            Elements = new EffectParameterCollection(cloneSource.Elements);
            StructureMembers = new EffectParameterCollection(cloneSource.StructureMembers);

            // Data is mutable, but a new copy happens during
            // boxing/unboxing so we can just assign it.
            Data = cloneSource.Data;
        }

		public string Name { get; private set; }

        public string Semantic { get; private set; }

		public EffectParameterClass ParameterClass { get; private set; }

		public EffectParameterType ParameterType { get; private set; }

		public int RowCount { get; private set; }

        public int ColumnCount { get; private set; }

        public EffectParameterCollection Elements { get; private set; }

        public EffectParameterCollection StructureMembers { get; private set; }

        public EffectAnnotationCollection Annotations { get; private set; }

        // TODO: Using object adds alot of boxing/unboxing overhead
        // and garbage generation.  We should consider a templated
        // type implementation to fix this!

        internal object Data { get; private set; }


		public void SetValue (object value)
		{
			throw new NotImplementedException();
		}

		public bool GetValueBoolean ()
		{
			throw new NotImplementedException();
		}

		public bool[] GetValueBooleanArray ()
		{
			throw new NotImplementedException();
		}

		public int GetValueInt32 ()
		{
            return (int)Data;
		}

		public int[] GetValueInt32Array ()
		{
			throw new NotImplementedException();
		}

		public Matrix GetValueMatrix ()
		{
			throw new NotImplementedException();
		}

		public Matrix[] GetValueMatrixArray (int count)
		{
			throw new NotImplementedException();
		}

		public Quaternion GetValueQuaternion ()
		{
			throw new NotImplementedException();
		}

		public Quaternion[] GetValueQuaternionArray ()
		{
			throw new NotImplementedException();
		}

		public Single GetValueSingle ()
		{
			switch(ParameterType) 
            {
			case EffectParameterType.Int32:
				return (Single)(int)Data;
			default:
				return (Single)Data;
			}
		}

		public Single[] GetValueSingleArray ()
		{
			if (Elements != null && Elements.Count > 0)
            {
                var ret = new Single[RowCount * ColumnCount * Elements.Count];
				for (int i=0; i<Elements.Count; i++)
                {
                    var elmArray = Elements[i].GetValueSingleArray();
                    for (var j = 0; j < elmArray.Length; j++)
						ret[RowCount*ColumnCount*i+j] = elmArray[j];
				}
				return ret;
			}
			
			switch(ParameterClass) 
            {
			case EffectParameterClass.Scalar:
				return new Single[] { GetValueSingle () };
            case EffectParameterClass.Vector:
			case EffectParameterClass.Matrix:
                    if (Data is Matrix)
                        return Matrix.ToFloatArray((Matrix)Data);
                    else
                        return (float[])Data;
			default:
				throw new NotImplementedException();
			}
		}

		public string GetValueString ()
		{
			throw new NotImplementedException();
		}

		public Texture2D GetValueTexture2D ()
		{
			return (Texture2D)Data;
		}

        // TODO: Add Texture3D support!
		//		public Texture3D GetValueTexture3D ()
		//		{
		//			return new Texture3D ();
		//		}

		public TextureCube GetValueTextureCube ()
		{
			throw new NotImplementedException();
		}

		public Vector2 GetValueVector2 ()
		{
            var vecInfo = (float[])Data;
			return new Vector2(vecInfo[0],vecInfo[1]);
		}

		public Vector2[] GetValueVector2Array ()
		{
			throw new NotImplementedException();
		}

		public Vector3 GetValueVector3 ()
		{
            var vecInfo = (float[])Data;
			return new Vector3(vecInfo[0],vecInfo[1],vecInfo[2]);

		}

		public Vector3[] GetValueVector3Array ()
		{
			throw new NotImplementedException();
		}

		public Vector4 GetValueVector4 ()
		{
            var vecInfo = (float[])Data;
			return new Vector4(vecInfo[0],vecInfo[1],vecInfo[2],vecInfo[3]);
		}

		public Vector4[] GetValueVector4Array ()
		{
			throw new NotImplementedException();
		}

		public void SetValue (bool value)
		{
			throw new NotImplementedException();
		}

		public void SetValue (bool[] value)
		{
			throw new NotImplementedException();
		}

		public void SetValue (int value)
		{
			Data = value;
		}

		public void SetValue (int[] value)
		{
			throw new NotImplementedException();
		}

		public void SetValue (Matrix value)
		{
            // TODO: Fix this to not work correctly in both cases!
#if NOMOJO
            data = value;
#else
			float[] matrixData = Matrix.ToFloatArray(Matrix.Transpose (value));
			for (int y=0; y<RowCount; y++) {
				for (int x=0; x<ColumnCount; x++) {
					((float[])data)[y*ColumnCount+x] = matrixData[y*4+x];
				}
			}
#endif
		}

		public void SetValue (Matrix[] value)
		{
            for (var i = 0; i < value.Length; i++)
				Elements[i].SetValue (value[i]);
		}

		public void SetValue (Quaternion value)
		{
			throw new NotImplementedException();
		}

		public void SetValue (Quaternion[] value)
		{
			throw new NotImplementedException();
		}

		public void SetValue (Single value)
		{
			switch (ParameterClass) 
            {
			case EffectParameterClass.Matrix:
			case EffectParameterClass.Vector:
				((float[])Data)[0] = value;
				break;
			case EffectParameterClass.Scalar:
				Data = value;
				break;
			default:
				throw new NotImplementedException();
			}
		}

		public void SetValue (Single[] value)
		{
			for (var i=0; i<value.Length; i++)
				Elements[i].SetValue (value[i]);
		}
		
		public void SetValue (string value)
		{
			throw new NotImplementedException();
		}

		public void SetValue (Texture value)
		{
			Data = value;
		}

		public void SetValue (Vector2 value)
		{
			Data = new float[2] { value.X, value.Y };
		}

		public void SetValue (Vector2[] value)
		{
            for (var i = 0; i < value.Length; i++)
				Elements[i].SetValue (value[i]);
		}

		public void SetValue (Vector3 value)
		{
			Data = new float[3] { value.X, value.Y, value.Z };
		}

		public void SetValue (Vector3[] value)
		{
            for (var i = 0; i < value.Length; i++)
				Elements[i].SetValue (value[i]);
		}

		public void SetValue (Vector4 value)
		{
			Data = new float[4] { value.X, value.Y, value.Z, value.W };
		}

		public void SetValue (Vector4[] value)
		{
            for (var i = 0; i < value.Length; i++)
				Elements[i].SetValue (value[i]);
		}
	}
}