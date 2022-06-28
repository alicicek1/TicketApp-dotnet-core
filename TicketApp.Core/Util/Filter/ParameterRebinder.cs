using System.Linq.Expressions;
using ExpressionVisitor = MongoDB.Bson.Serialization.ExpressionVisitor;

namespace TicketApp.Core.Util.Filter
{
    public class ParameterRebinder : ExpressionVisitor
    {
        private readonly ParameterExpression _parameter;

        public ParameterRebinder(ParameterExpression parameter)
        {
            this._parameter = parameter;
        }

        public Expression VisitParameter(ParameterExpression p)
        {
            return base.VisitParameter(this._parameter);
        }
    }
}