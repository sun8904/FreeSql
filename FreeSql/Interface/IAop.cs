﻿using FreeSql.DataAnnotations;
using FreeSql.DatabaseModel;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace FreeSql {
	public interface IAop {

		/// <summary>
		/// 监控 ToList 返回的的数据，用于拦截重新装饰
		/// </summary>
		EventHandler<AopToListEventArgs> ToList { get; set; }

		/// <summary>
		/// 监视 Where，包括 select/update/delete，可控制使上层不被执行。
		/// </summary>
		EventHandler<AopWhereEventArgs> Where { get; set; }

		/// <summary>
		/// 可自定义解析表达式
		/// </summary>
		EventHandler<AopParseExpressionEventArgs> ParseExpression { get; set; }

		/// <summary>
		/// 自定义实体的配置，方便和多个 ORM 共同使用
		/// </summary>
		EventHandler<AopConfigEntityEventArgs> ConfigEntity { get; set; }
		/// <summary>
		/// 自定义实体的属性配置，方便和多个 ORM 共同使用
		/// </summary>
		EventHandler<AopConfigEntityPropertyEventArgs> ConfigEntityProperty { get; set; }
	}

	public class AopToListEventArgs : EventArgs {
		public AopToListEventArgs(object list) {
			this.List = list;
		}
		/// <summary>
		/// 可重新装饰的引用数据
		/// </summary>
		public object List { get; }
	}
	public class AopWhereEventArgs : EventArgs {
		public AopWhereEventArgs(params object[] parameters) {
			this.Parameters = parameters;
		}
		public object[] Parameters { get; }
		/// <summary>
		/// 可使上层不被执行这个条件
		/// </summary>
		public bool IsCancel { get; set; }
	}
	public class AopParseExpressionEventArgs : EventArgs {
		public AopParseExpressionEventArgs(Expression expression, Func<Expression, string> freeParse) {
			this.Expression = expression;
			this.FreeParse = freeParse;
		}

		/// <summary>
		/// 内置解析功能，可辅助您进行解析
		/// </summary>
		public Func<Expression, string> FreeParse { get; }

		/// <summary>
		/// 需要您解析的表达式
		/// </summary>
		public Expression Expression { get; }
		/// <summary>
		/// 解析后的内容
		/// </summary>
		public string Result { get; set; }
	}
	public class AopConfigEntityEventArgs : EventArgs {
		public AopConfigEntityEventArgs(Type entityType) {
			this.EntityType = entityType;
			this.ModifyResult = new TableAttribute();
		}

		/// <summary>
		/// 实体类型
		/// </summary>
		public Type EntityType { get; }
		/// <summary>
		/// 实体配置
		/// </summary>
		public TableAttribute ModifyResult { get; }
	}
	public class AopConfigEntityPropertyEventArgs : EventArgs {
		public AopConfigEntityPropertyEventArgs(Type entityType, PropertyInfo property) {
			this.EntityType = entityType;
			this.Property = property;
			this.ModifyResult = new ColumnAttribute();
		}

		/// <summary>
		/// 实体类型
		/// </summary>
		public Type EntityType { get; }
		/// <summary>
		/// 实体的属性
		/// </summary>
		public PropertyInfo Property { get; }
		/// <summary>
		/// 实体的属性配置
		/// </summary>
		public ColumnAttribute ModifyResult { get; }
	}
}
