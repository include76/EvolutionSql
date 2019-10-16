create procedure uspParamDirection(
	@pIn int,
	@pInOut int output,
	@pOut int output
)
as
begin
	select @pInOut = @pIn * @pInOut
	select @pOut = @pIn * @pIn
	return 100
end