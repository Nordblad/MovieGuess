namespace MovieGuess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ToAzure : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Movies", "imdbRating", c => c.Double(nullable: false));
            DropColumn("dbo.Movies", "Rating");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Movies", "Rating", c => c.Double(nullable: false));
            DropColumn("dbo.Movies", "imdbRating");
        }
    }
}
