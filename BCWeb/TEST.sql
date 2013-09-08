select foo = (select up2.UserId from UserProfile up2
				join CompanyProfile cp2
				on up2.CompanyId = cp2.Id
				join webpages_UsersInRoles ur
				on ur.UserId = up2.UserId
				join webpages_Roles r
				on ur.RoleId = r.RoleId
				where r.RoleName='Manager'
				and cp2.Id = cp.Id),
* from UserProfile up
join CompanyProfile cp
on up.CompanyId = cp.Id

update UserProfile -- commit
set PostalCode = '98074'
where userid in (4,6)


delete CompanyProfile where id = 3

select * from UserProfile

declare @CompanyName as varchar(100), @Address1 as varchar,@Address2 as varchar,@City as varchar,@PostalCode as varchar
declare @StateId as int, @CountyId as int,@Phone as int,@OperatingDistance as int,@BusinessTypeId as int, @UserId as int, @CompanyId as int
declare @Published as bit, @exists as bit
declare comp_cur Cursor for select UserId, CompanyName,Address1,Address2,City,PostalCode,StateId,CountyId,Phone,Published,OperatingDistance,BusinessTypeId from UserProfile where email <>'admin'
open comp_cur
fetch next from comp_cur into @UserId,@CompanyName,@Address1,@Address2,@City,@PostalCode,@StateId,@CountyId,@Phone,@Published,@OperatingDistance,@BusinessTypeId
while @@FETCH_STATUS = 0
begin
select @exists = 1			
            from UserProfile
			where email <>'admin' and
			CompanyName = @CompanyName and 
            Phone = @Phone and 
            BusinessTypeId = @BusinessTypeId
print @exists
fetch next from comp_cur into @UserId,@CompanyName,@Address1,@Address2,@City,@PostalCode,@StateId,@CountyId,@Phone,@Published,@OperatingDistance,@BusinessTypeId
end
close comp_cur
deallocate comp_cur

select * From userprofile
select * from CompanyProfile