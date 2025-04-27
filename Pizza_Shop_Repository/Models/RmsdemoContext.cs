using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Pizza_Shop_Repository.Models;

public partial class RmsdemoContext : DbContext
{
    public RmsdemoContext()
    {
    }

    public RmsdemoContext(DbContextOptions<RmsdemoContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<City> Cities { get; set; }

    public virtual DbSet<Country> Countries { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Customerhistory> Customerhistories { get; set; }

    public virtual DbSet<FavroiteItem> FavroiteItems { get; set; }

    public virtual DbSet<Feedback> Feedbacks { get; set; }

    public virtual DbSet<InvoceTax> InvoceTaxes { get; set; }

    public virtual DbSet<Invoice> Invoices { get; set; }

    public virtual DbSet<Item> Items { get; set; }

    public virtual DbSet<ItemModifierGroup> ItemModifierGroups { get; set; }

    public virtual DbSet<Kot> Kots { get; set; }

    public virtual DbSet<Modifier> Modifiers { get; set; }

    public virtual DbSet<ModifierModifierGroupMapping> ModifierModifierGroupMappings { get; set; }

    public virtual DbSet<Modifiergroup> Modifiergroups { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderItem> OrderItems { get; set; }

    public virtual DbSet<OrderItemModifier> OrderItemModifiers { get; set; }

    public virtual DbSet<OrderPayment> OrderPayments { get; set; }

    public virtual DbSet<OrderTableMapping> OrderTableMappings { get; set; }

    public virtual DbSet<Permission> Permissions { get; set; }

    public virtual DbSet<Profile> Profiles { get; set; }

    public virtual DbSet<Rating> Ratings { get; set; }

    public virtual DbSet<Resetpassword> Resetpasswords { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Roleandpermission> Roleandpermissions { get; set; }

    public virtual DbSet<Section> Sections { get; set; }

    public virtual DbSet<Stable> Stables { get; set; }

    public virtual DbSet<State> States { get; set; }

    public virtual DbSet<Tax> Taxes { get; set; }

    public virtual DbSet<UserLogin> UserLogins { get; set; }

    public virtual DbSet<UserRole> UserRoles { get; set; }

    public virtual DbSet<Waitinglist> Waitinglists { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=Localhost;Database=RMS2;Username=postgres;password=root");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasPostgresEnum("item_type", new[] { "vegiterian", "Non-veterian", "vegan", "Non-Vegiterian" })
            .HasPostgresEnum("paymentmode_type", new[] { "online", "cash", "card" })
            .HasPostgresEnum("role_type", new[] { "super admin", "admin", "chef" })
            .HasPostgresEnum("tax_type", new[] { "percentage", "fixed amount" });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Categoryid).HasName("category_pkey");
        });

        modelBuilder.Entity<City>(entity =>
        {
            entity.HasKey(e => e.Cityid).HasName("city_pkey");

            entity.HasOne(d => d.State).WithMany(p => p.Cities)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_stateid");
        });

        modelBuilder.Entity<Country>(entity =>
        {
            entity.HasKey(e => e.Countryid).HasName("country_pkey");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.Customerid).HasName("customer_pkey");
        });

        modelBuilder.Entity<Customerhistory>(entity =>
        {
            entity.HasKey(e => e.Cutomerhistoryid).HasName("customerhistory_pkey");

            entity.Property(e => e.Cutomerhistoryid).UseIdentityAlwaysColumn();

            entity.HasOne(d => d.Customer).WithMany(p => p.Customerhistories).HasConstraintName("fk_customerid");
        });

        modelBuilder.Entity<FavroiteItem>(entity =>
        {
            entity.HasKey(e => e.Faveroititemid).HasName("favroite_item_pkey");

            entity.Property(e => e.Faveroititemid).UseIdentityAlwaysColumn();

            entity.HasOne(d => d.Category).WithMany(p => p.FavroiteItems)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_categoryid");

            entity.HasOne(d => d.Item).WithMany(p => p.FavroiteItems)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_itemid");
        });

        modelBuilder.Entity<Feedback>(entity =>
        {
            entity.HasKey(e => e.Feedbackid).HasName("feedback_pkey");

            entity.Property(e => e.Feedbackid).UseIdentityAlwaysColumn();

            entity.HasOne(d => d.Customer).WithMany(p => p.Feedbacks)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_cutomerId");

            entity.HasOne(d => d.Order).WithMany(p => p.Feedbacks)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_orderId");
        });

        modelBuilder.Entity<InvoceTax>(entity =>
        {
            entity.HasKey(e => e.InvoiceTaxId).HasName("Invoce_Tax_pkey");

            entity.HasOne(d => d.Order).WithMany(p => p.InvoceTaxes)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_orderid");

            entity.HasOne(d => d.Tax).WithMany(p => p.InvoceTaxes)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_taxId");
        });

        modelBuilder.Entity<Invoice>(entity =>
        {
            entity.HasKey(e => e.Invoiceid).HasName("invoice_pkey");

            entity.Property(e => e.Invoiceid).UseIdentityAlwaysColumn();

            entity.HasOne(d => d.Order).WithMany(p => p.Invoices).HasConstraintName("invoice_orderid_fkey");
        });

        modelBuilder.Entity<Item>(entity =>
        {
            entity.HasKey(e => e.Itemid).HasName("item_pkey");

            entity.HasOne(d => d.Category).WithMany(p => p.Items)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_categoryid");
        });

        modelBuilder.Entity<ItemModifierGroup>(entity =>
        {
            entity.HasKey(e => e.ItemMgid).HasName("Item_modifier_group_pkey");

            entity.HasOne(d => d.Item).WithMany(p => p.ItemModifierGroups)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_itemid");

            entity.HasOne(d => d.ModifierGroup).WithMany(p => p.ItemModifierGroups)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_mgId");
        });

        modelBuilder.Entity<Kot>(entity =>
        {
            entity.HasKey(e => e.Kotid).HasName("kot_pkey");

            entity.HasOne(d => d.Category).WithMany(p => p.Kots)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_categoryid");

            entity.HasOne(d => d.Order).WithMany(p => p.Kots)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_orderid");
        });

        modelBuilder.Entity<Modifier>(entity =>
        {
            entity.HasKey(e => e.Modifierid).HasName("modifier_pkey");
        });

        modelBuilder.Entity<ModifierModifierGroupMapping>(entity =>
        {
            entity.HasKey(e => e.ModifierModifierGroupId).HasName("modifier_modifier_group_mapping_pkey");

            entity.HasOne(d => d.ModifierGroup).WithMany(p => p.ModifierModifierGroupMappings)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_ModifierGroupId");

            entity.HasOne(d => d.Modifier).WithMany(p => p.ModifierModifierGroupMappings)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_ModifierId");
        });

        modelBuilder.Entity<Modifiergroup>(entity =>
        {
            entity.HasKey(e => e.Modifiergroupid).HasName("modifiergroup_pkey");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Orderid).HasName("orders_pkey");

            entity.HasOne(d => d.Customer).WithMany(p => p.Orders)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_CustomerId");

            entity.HasOne(d => d.OrderPayment).WithMany(p => p.Orders).HasConstraintName("fk_PaymentId");

            entity.HasOne(d => d.Rating).WithMany(p => p.Orders).HasConstraintName("fk_RattingId");
        });

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasKey(e => e.Orderitemid).HasName("order_item_pkey");

            entity.Property(e => e.Isdeleted).HasDefaultValueSql("false");

            entity.HasOne(d => d.Item).WithMany(p => p.OrderItems)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_itemid");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderItems)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_OrderId");
        });

        modelBuilder.Entity<OrderItemModifier>(entity =>
        {
            entity.HasKey(e => e.OrderItemModifierId).HasName("Order_Item_Modifier_pkey");

            entity.Property(e => e.Isdeleted).HasDefaultValueSql("false");

            entity.HasOne(d => d.ModifierGroup).WithMany(p => p.OrderItemModifiers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_ModifierGroupId");

            entity.HasOne(d => d.Modifier).WithMany(p => p.OrderItemModifiers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_ModifierId");

            entity.HasOne(d => d.OrderItem).WithMany(p => p.OrderItemModifiers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_orderitem");
        });

        modelBuilder.Entity<OrderPayment>(entity =>
        {
            entity.HasKey(e => e.PaymentId).HasName("order_payment_pkey");
        });

        modelBuilder.Entity<OrderTableMapping>(entity =>
        {
            entity.HasKey(e => e.OrderTableId).HasName("order_table_mapping_pkey");

            entity.HasOne(d => d.Customer).WithMany(p => p.OrderTableMappings)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_customerid");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderTableMappings).HasConstraintName("fk_orderId");

            entity.HasOne(d => d.Table).WithMany(p => p.OrderTableMappings)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_tableId");
        });

        modelBuilder.Entity<Permission>(entity =>
        {
            entity.HasKey(e => e.Permissionid).HasName("permissions_pkey");
        });

        modelBuilder.Entity<Profile>(entity =>
        {
            entity.HasKey(e => e.Userid).HasName("profile_pkey");

            entity.Property(e => e.Createdat).HasDefaultValueSql("now()");
            entity.Property(e => e.Isdeleted).HasDefaultValueSql("false");
            entity.Property(e => e.Status).HasDefaultValueSql("true");

            entity.HasOne(d => d.City).WithMany(p => p.Profiles)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("profile_cityid_fkey");

            entity.HasOne(d => d.Country).WithMany(p => p.Profiles)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("profile_countryid_fkey");

            entity.HasOne(d => d.State).WithMany(p => p.Profiles)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("profile_stateid_fkey");
        });

        modelBuilder.Entity<Rating>(entity =>
        {
            entity.HasKey(e => e.Ratindid).HasName("rating_pkey");

            entity.Property(e => e.Avgratting).HasComputedColumnSql("(((food + ambiance) + service) / 3)", true);
        });

        modelBuilder.Entity<Resetpassword>(entity =>
        {
            entity.HasKey(e => e.Resetpasswordid).HasName("resetpassword_pkey");

            entity.Property(e => e.Resetpasswordid).UseIdentityAlwaysColumn();
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Roleid).HasName("roles_pkey");
        });

        modelBuilder.Entity<Roleandpermission>(entity =>
        {
            entity.HasKey(e => e.Rpid).HasName("roleandpermission_pkey");

            entity.Property(e => e.Addandeditpermission).HasDefaultValueSql("true");
            entity.Property(e => e.Isdeletepermission).HasDefaultValueSql("true");
            entity.Property(e => e.Viewpermission).HasDefaultValueSql("true");

            entity.HasOne(d => d.Permission).WithMany(p => p.Roleandpermissions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_permissionid");

            entity.HasOne(d => d.Role).WithMany(p => p.Roleandpermissions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_roleid");
        });

        modelBuilder.Entity<Section>(entity =>
        {
            entity.HasKey(e => e.Sectionid).HasName("sections_pkey");
        });

        modelBuilder.Entity<Stable>(entity =>
        {
            entity.HasKey(e => e.Tableid).HasName("stables_pkey");

            entity.HasOne(d => d.Section).WithMany(p => p.Stables)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_sectionid");
        });

        modelBuilder.Entity<State>(entity =>
        {
            entity.HasKey(e => e.Stateid).HasName("states_pkey");

            entity.HasOne(d => d.Country).WithMany(p => p.States)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_countryid");
        });

        modelBuilder.Entity<Tax>(entity =>
        {
            entity.HasKey(e => e.Taxid).HasName("tax_pkey");
        });

        modelBuilder.Entity<UserLogin>(entity =>
        {
            entity.HasKey(e => e.Userloginid).HasName("user_login_pkey");

            entity.Property(e => e.Isdeleted).HasDefaultValueSql("false");

            entity.HasOne(d => d.User).WithMany(p => p.UserLogins)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("user_login_userid_fkey");
        });

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.HasKey(e => e.Userroleid).HasName("user_role_pkey");

            entity.HasOne(d => d.Role).WithMany(p => p.UserRoles).HasConstraintName("fk_roleid");

            entity.HasOne(d => d.User).WithMany(p => p.UserRoles)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_userid");
        });

        modelBuilder.Entity<Waitinglist>(entity =>
        {
            entity.HasKey(e => e.Waitinglistid).HasName("waitinglist_pkey");

            entity.HasOne(d => d.Customer).WithMany(p => p.Waitinglists)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_customerid");

            entity.HasOne(d => d.Section).WithMany(p => p.Waitinglists)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_sectionid");

            entity.HasOne(d => d.Table).WithMany(p => p.Waitinglists).HasConstraintName("fk_tableid");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
