if not exists (select 1 from master.dbo.sysdatabases db where db.name='Assignment') 
begin
    create database Assignment
end
else

begin
    print ('Assignment database already exists');
end
go

use Assignment
go
if not exists (select 1 from  information_schema.tables t where t.TABLE_NAME='Providers' and t.TABLE_SCHEMA='dbo')
begin
	create table dbo.Providers
	(
	   ProviderId int not null identity(1,1) constraint pk_providers_id primary key clustered,
	   FirstName varchar(100) not null,
	   LastName varchar(100) not null,
	   NPINumber varchar(10) not null,
	   Phone varchar(15) not null,
	   Email varchar(30) not null,
	   created_on datetime not null constraint   df_providers_created_on  default  getdate()
	)
	print('table dbo.Providers created successfully.')
end
go
if not exists (select 1 from  information_schema.tables t where t.TABLE_NAME='States' and t.TABLE_SCHEMA='dbo')
begin
	create table dbo.States
	(
	   StateId int not null identity(1,1) constraint pk_states_id primary key clustered,
	   Name varchar(25) not null,
	   Code varchar(2) not null,
	   created_on datetime not null constraint   df_states_created_on  default  getdate()
	)
	print('table dbo.States created successfully.')
end
go
if not exists (select 1 from  information_schema.tables t where t.TABLE_NAME='AddressType' and t.TABLE_SCHEMA='dbo')
begin
	create table dbo.AddressType
	(
	   AddressTypeId int not null identity(1,1) constraint pk_addresstype_id primary key clustered,
	   Name varchar(10) not null,
	   created_on datetime not null constraint   df_addresstype_created_on  default  getdate()
	)
	print('table dbo.AddressType created successfully.')
end
if not exists (select 1 from  information_schema.tables t where t.TABLE_NAME='ProviderAddress' and t.TABLE_SCHEMA='dbo')
begin
	create table dbo.ProviderAddress
	(
	   ProviderAddressId int not null identity(1,1) constraint pk_provideraddress_id primary key clustered,
	   ProviderId int not null constraint fk_provider_address_providerid  foreign key references dbo.Providers(ProviderId),
	   Address1 varchar(100) not null,
	   City varchar(20) not null,
	   StateId int not null constraint fk_provider_address_stateid  foreign key references dbo.States(StateId),
	   ZipCode varchar(10) not null,
	   AddressTypeId int not null constraint fk_provider_address_address_type_id  foreign key references dbo.AddressType(AddressTypeId),
	   created_on datetime not null constraint   df_provider_address_created_on  default  getdate()
	)
	print('table dbo.ProviderAddress created successfully.')
end

if not exists (select 1 from  sys.types t where t.is_table_type=1 and t.name='StateUdt')
begin
	create type StateUdt as table
	(	  
	   Name varchar(25) not null,
	   Code varchar(2) not null
	)
	print('User defined table type dbo.StateUdt created successfully.')
end
go

--Lookup values for dbo.AddressType


insert into dbo.AddressType
(
   Name
)
values
(
  'Business'
),
(
   'Shipping'
),
(
  'Billing'
)

--============================================Stored Procedures======================================
Use Assignment
go
If  exists (select 1 from sys.objects where object_id = object_id('dbo.CreateProvider'))
begin
    drop procedure 	dbo.CreateProvider
end
go
create procedure dbo.CreateProvider
(   
   @FirstName varchar(100),
   @LastName varchar(100),
   @NPINumber varchar(10),
   @Phone varchar(15),
   @Email varchar(30),
   @ProviderId int output
)
as
begin
	insert into dbo.Providers
	(
	   FirstName,
	   LastName,
	   NPINumber,
	   Phone,
	   Email
	 )
	 values
	 (
	     @FirstName,
		 @LastName,
		 @NPINumber,
		 @Phone,
		 @Email
	 )

	 select @ProviderId  =scope_identity();
end
go

If  exists (select 1 from sys.objects where object_id = object_id('dbo.CreateProviderAddress'))
begin
    drop procedure 	dbo.CreateProviderAddress
end
go
create procedure dbo.CreateProviderAddress
(   
   @ProviderId int,
   @Address1 varchar(100),
   @City varchar(20),
   @StateId int,
   @ZipCode varchar(10),
   @AddressTypeId int   
)
as
begin
	insert into dbo.ProviderAddress
	(
	   ProviderId,
	   Address1,
	   City,
	   StateId,
	   ZipCode,
	   AddressTypeId
	 )
	 values
	 (
	    @ProviderId ,
		@Address1,
		@City,
		@StateId,
		@ZipCode,
        @AddressTypeId
	 )
	 
end
go
If  exists (select 1 from sys.objects where object_id = object_id('dbo.GetAllProviders'))
begin
    drop procedure 	dbo.GetAllProviders
end
go
create procedure dbo.GetAllProviders
as
begin
	select
		p.FirstName,
		p.LastName,
		p.NPINumber,
		p.Phone,
		p.ProviderId,
		p.FirstName + ' ' + p.LastName as FullName
	from
		dbo.Providers p
	order by
		FullName
	asc
end
go
If  exists (select 1 from sys.objects where object_id = object_id('dbo.GetProviderById'))
begin
    drop procedure 	dbo.GetProviderById
end
go
create procedure dbo.GetProviderById
(
	@ProviderId int
)
as
begin
	select
		p.FirstName,
		p.LastName,
		p.NPINumber,
		p.Phone,
		p.ProviderId
	from
		dbo.Providers p
		
	where
		p.ProviderId=@ProviderId
	
end
go
If  exists (select 1 from sys.objects where object_id = object_id('dbo.GetProviderAddressByProviderId'))
begin
    drop procedure 	dbo.GetProviderAddressByProviderId
end
go
create procedure dbo.GetProviderAddressByProviderId
(
	@ProviderId int
)
as
begin
	select		
		pa.Address1,
		pa.City,
		st.Name,
		pa.ZipCode
	from
		dbo.Providers p
		inner join dbo.ProviderAddress pa on
			pa.ProviderId = p.ProviderId
		inner join dbo.AddressType pat on
			pa.AddressTypeId = pat.AddressTypeId
		inner join dbo.States st on
			pa.StateId = st.StateId
	where
		p.ProviderId=@ProviderId and
		pat.Name = 'business'
	
end
go
If  exists (select 1 from sys.objects where object_id = object_id('dbo.CreateStates'))
begin
    drop procedure 	dbo.CreateStates
end
go
create procedure dbo.CreateStates
(   
    @StateUdt StateUdt readonly
)
as
begin
	insert into dbo.States
	(
	   Name,
	   Code
	 )
	 select
	     Name,
		 Code
	 from
		@StateUdt
end
go
If  exists (select 1 from sys.objects where object_id = object_id('dbo.GetAllStates'))
begin
    drop procedure 	dbo.GetAllStates
end
go
create procedure dbo.GetAllStates

as
begin
    select
	    StateId as Id,
		Name
	from
		dbo.States
	order by
		Name
	asc
end
go